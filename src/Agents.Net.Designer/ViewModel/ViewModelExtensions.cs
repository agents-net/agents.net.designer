using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.ViewModel
{
    public static class ViewModelExtensions
    {
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
            List<T> items = new List<T>();
            Stack<TreeViewItem> unvisited = new Stack<TreeViewItem>(new []{community});
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
            Stack<TreeViewItem> unvisited = new Stack<TreeViewItem>(new []{community});
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
                Stack<List<TreeViewItem>> unvisitedPaths = new Stack<List<TreeViewItem>>(new []{new List<TreeViewItem>(new []{viewModel}), });
                while (unvisitedPaths.Any())
                {
                    List<TreeViewItem> currentPath = unvisitedPaths.Pop();
                    TreeViewItem currentParent = currentPath[^1];
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
            FolderViewModel root = ns.StartsWith('.') ?  relativeRoot : viewModel;
            string[] path = ns.Split('.', StringSplitOptions.RemoveEmptyEntries);
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

        public static MessageViewModel CreateViewModel(this MessageModel message)
        {
            return new MessageViewModel
            {
                Name = message.Name,
                FullName = message.FullName(),
                RelativeNamespace = message.Namespace,
                ModelId = message.Id,
                BuildIn = message.BuildIn
            };
        }

        public static AgentViewModel CreateViewModel(this AgentModel agent, CommunityViewModel community)
        {
            AvailableItemsViewModel availableItems = community.FindItemByType<AgentViewModel>()?.AvailableItems
                                                     ?? new AvailableItemsViewModel
                                                     {
                                                         AvailableMessages =
                                                             new ObservableCollection<MessageViewModel>(
                                                                 community.FindItemsByType<MessageViewModel>()
                                                                          .Concat(community.BuildInTypes.OfType<MessageViewModel>()))
                                                     };

            return agent.CreateViewModel(availableItems);
        }

        public static AgentViewModel CreateViewModel(this AgentModel agent, AvailableItemsViewModel availableViewModel)
        {
            return new AgentViewModel
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
            
            IEnumerable<MessageViewModel> CollectMessages(Guid[] agentMessages)
            {
                List<MessageViewModel> viewModels = new List<MessageViewModel>();
                foreach (Guid messageDefinition in agentMessages)
                {
                    viewModels.Add(availableViewModel.AvailableMessages.First(m => m.ModelId == messageDefinition));
                }

                return viewModels;
            }
        }
    }
}