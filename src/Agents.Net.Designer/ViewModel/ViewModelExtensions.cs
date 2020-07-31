using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Agents.Net.Designer.ViewModel
{
    public static class ViewModelExtensions
    {
        public static void AddItem(this CommunityViewModel viewModel, AgentViewModel agentViewModel)
        {
            viewModel.AddItem(agentViewModel,agentViewModel.Namespace);
        }
        
        public static void AddItem(this CommunityViewModel viewModel, MessageViewModel messageViewModel)
        {
            viewModel.AddItem(messageViewModel,messageViewModel.Namespace);
        }

        public static TreeViewItem FindViewItemById(this CommunityViewModel community, Guid id)
        {
            Stack<TreeViewItem> unvisited = new Stack<TreeViewItem>(new []{community});
            while (unvisited.Any())
            {
                TreeViewItem item = unvisited.Pop();
                if (item is AgentViewModel viewModel &&
                    viewModel.ModelId == id)
                {
                    return viewModel;
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
                if (parent.Items.Any())
                {
                    break;
                }
                child = parent;
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
            Dictionary<string, FolderViewModel> rootNames = viewModel.Items
                                                                     .OfType<FolderViewModel>()
                                                                     .ToDictionary(i => i.Name,
                                                                                   i => i);
            string rootName = rootNames.Keys.FirstOrDefault(ns.StartsWith) ?? string.Empty;
            FolderViewModel root = string.IsNullOrEmpty(rootName) ? viewModel : rootNames[rootName];
            string[] path = ns.Substring(rootName.Length)
                              .Split('.', StringSplitOptions.RemoveEmptyEntries);
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

        public static MessageViewModel GenerateMessageMock(this string messageDefinition, ObservableCollection<MessageViewModel> availableMessages)
        {
            MessageViewModel viewModel = availableMessages.FirstOrDefault(m => m.FullName == messageDefinition &&
                                                                               m.ModelId == default);
            if (viewModel == null)
            {
                viewModel = new MessageViewModel
                {
                    Name = messageDefinition.Substring(messageDefinition.LastIndexOf('.') + 1),
                    FullName = messageDefinition,
                    Namespace = messageDefinition.Substring(0, Math.Max(0, messageDefinition.LastIndexOf('.')))
                };
                availableMessages.Add(viewModel);
            }

            return viewModel;
        }
    }
}