using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModelLoaded))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Produces(typeof(TreeViewModelUpdated))]
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
            OnMessage(new TreeViewModelUpdated(set));
            
            CommunityViewModel GenerateCommunityViewModel()
            {
                CommunityModel model = set.Message1.Model;
                string rootNamespace = model.GeneratorSettings.PackageNamespace;
                CommunityViewModel root = new()
                {
                    Name = string.IsNullOrEmpty(rootNamespace)?"<Root>":rootNamespace,
                    GenerateAutofacModule = model.GeneratorSettings.GenerateAutofacModule
                };
                if (!string.IsNullOrEmpty(rootNamespace))
                {
                    FolderViewModel folder = new()
                    {
                        Name = rootNamespace,
                        IsRelativeRoot = true
                    };
                    root.Items.Add(folder);
                }
                
                AvailableItemsViewModel availableViewModel = new();
                foreach (MessageModel message in model.Messages)
                {
                    MessageViewModel messageViewModel = message.CreateViewModel(availableViewModel);
                    root.AddItem(messageViewModel);
                    availableViewModel.AvailableMessages.Add(messageViewModel);
                }

                foreach (AgentModel agent in model.Agents)
                {
                    AgentViewModel agentViewModel = agent.CreateViewModel(availableViewModel);
                    root.AddItem(agentViewModel);
                    availableViewModel.AvailableAgents.Add(agentViewModel);
                }

                return root;
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}