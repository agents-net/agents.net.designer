using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModelLoaded))]
    [Consumes(typeof(TreeViewModelCreated))]
    public class TreeViewModelBuilder : Agent
    {
        private readonly MessageCollector<ModelLoaded, TreeViewModelCreated> collector;
        
        public TreeViewModelBuilder(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<ModelLoaded, TreeViewModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModelLoaded, TreeViewModelCreated> set)
        {
            TreeViewModel viewModel = set.Message2.ViewModel;
            CommunityViewModel newCommunity = GenerateCommunityViewModel();
            viewModel.Community = newCommunity;
            
            CommunityViewModel GenerateCommunityViewModel()
            {
                CommunityModel model = set.Message1.Model;
                string rootNamespace = model.GeneratorSettings?.PackageNamespace ?? string.Empty;
                CommunityViewModel root = new CommunityViewModel
                {
                    Name = string.IsNullOrEmpty(rootNamespace) ? "<Root>" : rootNamespace
                };
                
                foreach (AgentModel agent in model.Agents)
                {
                    string ns = agent.Namespace.ExtendNamespace(model);
                    string[] path = ns.StartsWith(rootNamespace)
                                        ? ns.Substring(rootNamespace.Length)
                                            .Split('.', StringSplitOptions.RemoveEmptyEntries)
                                        : ns.Split('.', StringSplitOptions.RemoveEmptyEntries);
                    FolderViewModel folder = GeneratePath(path);
                    folder.Items.Add(new AgentViewModel
                    {
                        Name = agent.Name,
                        FullName = agent.FullName(model)
                    });
                }

                foreach (MessageModel message in model.Messages)
                {
                    string ns = message.Namespace.ExtendNamespace(model);
                    string[] path = ns.StartsWith(rootNamespace)
                                        ? ns.Substring(rootNamespace.Length)
                                            .Split('.', StringSplitOptions.RemoveEmptyEntries)
                                        : ns.Split('.', StringSplitOptions.RemoveEmptyEntries);
                    FolderViewModel folder = GeneratePath(path);
                    folder.Items.Add(new MessageViewModel
                    {
                        Name = message.Name,
                        FullName = message.FullName(model)
                    });
                }

                return root;
                
                FolderViewModel GeneratePath(IEnumerable<string> path)
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
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}