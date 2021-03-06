#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.ViewModel
{
    public static class ViewModelExtensions
    {
        public static void Select(this TreeViewItem selectable)
        {
            TreeViewItem expandable = selectable.Parent;
            while (expandable != null)
            {
                expandable.IsExpanded = true;
                expandable = expandable.Parent;
            }
            
            selectable.IsSelected = true;
        }
        
        public static void AddItem(this CommunityViewModel viewModel, AgentViewModel agentViewModel)
        {
            viewModel.AddItem(agentViewModel,agentViewModel.RelativeNamespace);
        }
        
        public static void AddItem(this CommunityViewModel viewModel, MessageViewModel messageViewModel)
        {
            if (messageViewModel.BuildIn)
            {
                viewModel.BuildInTypes.Add(messageViewModel);
                return;
            }
            viewModel.AddItem(messageViewModel,messageViewModel.RelativeNamespace);
        }

        public static T FindItemByType<T>(this CommunityViewModel community)
            where T : TreeViewItem
        {
            return community.FindItemsByType<T>().FirstOrDefault();
        }

        public static IEnumerable<T> FindItemsByType<T>(this CommunityViewModel community)
            where T : TreeViewItem
        {
            List<T> items = new();
            Stack<TreeViewItem> unvisited = new(new []{community});
            while (unvisited.Any())
            {
                TreeViewItem item = unvisited.Pop();
                if (item is T value)
                {
                    items.Add(value);
                }

                foreach (TreeViewItem child in item.Items)
                {
                    unvisited.Push(child);
                }
            }

            return items;
        }

        public static TreeViewItem FindViewItemById(this CommunityViewModel community, Guid id)
        {
            Stack<TreeViewItem> unvisited = new(new []{community});
            while (unvisited.Any())
            {
                TreeViewItem item = unvisited.Pop();
                if (item is AgentViewModel agent &&
                    agent.ModelId == id)
                {
                    return agent;
                }
                else if (item is MessageViewModel message &&
                         message.ModelId == id)
                {
                    return message;
                }

                foreach (TreeViewItem child in item.Items)
                {
                    unvisited.Push(child);
                }
            }
            throw new InvalidOperationException($"No view model found for model {id}.");
        }
        
        public static void RemoveItem(this CommunityViewModel viewModel, TreeViewItem viewItem)
        {
            List<TreeViewItem> viewItemPath = FindViewItem();
            viewItemPath.Reverse();
            TreeViewItem child = viewItemPath[0];
            viewItemPath.RemoveAt(0);
            foreach (TreeViewItem parent in viewItemPath)
            {
                parent.Items.Remove(child);
                if (IsIndestructible(parent))
                {
                    break;
                }
                child = parent;
            }

            static bool IsIndestructible(TreeViewItem item)
            {
                return item.Items.Any() || 
                       (item is FolderViewModel folder &&
                        folder.IsRelativeRoot);
            }

            List<TreeViewItem> FindViewItem()
            {
                Stack<List<TreeViewItem>> unvisitedPaths = new(new []{new List<TreeViewItem>(new []{viewModel}), });
                while (unvisitedPaths.Any())
                {
                    List<TreeViewItem> currentPath = unvisitedPaths.Pop();
                    TreeViewItem currentParent = currentPath[currentPath.Count-1];
                    if (currentParent == viewItem)
                    {
                        return currentPath;
                    }

                    foreach (TreeViewItem treeViewItem in currentParent.Items)
                    {
                        unvisitedPaths.Push(new List<TreeViewItem>(currentPath.Concat(new []{treeViewItem})));
                    }
                }
                throw new InvalidOperationException($"Could not find view model item {viewItem} in view model.");
            }
        }

        private static void AddItem(this CommunityViewModel viewModel, TreeViewItem viewItem, string ns)
        {
            FolderViewModel relativeRoot = viewModel.Items.OfType<FolderViewModel>()
                                                    .FirstOrDefault(f => f.IsRelativeRoot)
                                           ?? viewModel;
            FolderViewModel root = ns.StartsWith(".", StringComparison.Ordinal) ?  relativeRoot : viewModel;
            string[] path = ns.Split(new []{'.'}, StringSplitOptions.RemoveEmptyEntries);
            FolderViewModel parent = GeneratePath();
            parent.Items.Add(viewItem);
            
            FolderViewModel GeneratePath()
            {
                FolderViewModel result = root;
                foreach (string part in path)
                {
                    FolderViewModel child = result.Items.OfType<FolderViewModel>()
                                                  .FirstOrDefault(f => f.Name == part);
                    if (child == null)
                    {
                        child = new FolderViewModel{Name = part};
                        result.Items.Add(child);
                    }

                    result = child;
                }

                return result;
            }
        }

        public static MessageViewModel CreateViewModel(this MessageModel message, CommunityViewModel community)
        {
            AvailableItemsViewModel availableItems = FindOrCreateAvailableItems(community);
            return message.CreateViewModel(availableItems);
        }

        public static MessageViewModel CreateViewModel(this MessageModel message, AvailableItemsViewModel availableViewModel)
        {
            MessageViewModel viewModel = new()
            {
                Name = message.Name,
                FullName = message.FullName(),
                RelativeNamespace = message.Namespace,
                ModelId = message.Id,
                BuildIn = message.BuildIn,
                IsGeneric = message.IsGeneric,
                GenericParameterCount = message.GenericParameterCount,
                IsGenericInstance = message.IsGenericInstance,
                AvailableItems = availableViewModel
            };
            if (message is MessageDecoratorModel decoratorModel)
            {
                viewModel.MessageType = MessageType.MessageDecorator;
                viewModel.DecoratedMessage = availableViewModel.AvailableMessages.FirstOrDefault(m => m.ModelId == decoratorModel.DecoratedMessage);
            }
            else
            {
                viewModel.MessageType = MessageType.Message;
            }
            return viewModel;
        }

        private static AvailableItemsViewModel FindOrCreateAvailableItems(CommunityViewModel community)
        {
            AvailableItemsViewModel availableItems = community.FindItemByType<AgentViewModel>()?.AvailableItems
                                                     ?? community.FindItemByType<MessageViewModel>()?.AvailableItems
                                                     ?? new AvailableItemsViewModel
                                                     (
                                                         community.FindItemsByType<MessageViewModel>()
                                                                  .Concat(community.BuildInTypes
                                                                              .OfType<MessageViewModel>()),
                                                         community.FindItemsByType<AgentViewModel>()
                                                                  .Concat(community.BuildInTypes.OfType<AgentViewModel>())
                                                     );
            return availableItems;
        }

        public static AgentViewModel CreateViewModel(this AgentModel agent, CommunityViewModel community)
        {
            AvailableItemsViewModel availableItems = FindOrCreateAvailableItems(community);
            return agent.CreateViewModel(availableItems);
        }

        public static AgentViewModel CreateViewModel(this AgentModel agent, AvailableItemsViewModel availableViewModel)
        {
            AgentViewModel viewModel = new()
            {
                Name = agent.Name,
                FullName = agent.FullName(),
                RelativeNamespace = agent.Namespace,
                IncomingEvents = new ObservableCollection<string>(agent.IncomingEvents),
                ProducedEvents = new ObservableCollection<string>(agent.ProducedEvents),
                ConsumingMessages =
                    new ObservableCollection<MessageViewModel>(CollectMessages(agent.ConsumingMessages)),
                ProducingMessages = new ObservableCollection<MessageViewModel>(CollectMessages(agent.ProducedMessages)),
                AvailableItems = availableViewModel,
                ModelId = agent.Id
            };
            if (agent is InterceptorAgentModel interceptor)
            {
                viewModel.InterceptingMessages = new ObservableCollection<MessageViewModel>(CollectMessages(interceptor.InterceptingMessages));
                viewModel.AgentType = AgentType.Interceptor;
            }
            else
            {
                viewModel.AgentType = AgentType.Agent;
            }
            return viewModel;
            
            IEnumerable<MessageViewModel> CollectMessages(Guid[] agentMessages)
            {
                List<MessageViewModel> viewModels = new();
                foreach (Guid messageDefinition in agentMessages)
                {
                    viewModels.Add(availableViewModel.AvailableMessages.First(m => m.ModelId == messageDefinition));
                }

                return viewModels;
            }
        }
    }
}