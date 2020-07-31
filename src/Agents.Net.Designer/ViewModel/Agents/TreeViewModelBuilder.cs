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
                    Name = string.IsNullOrEmpty(rootNamespace)?"<Root>":rootNamespace
                };
                if (!string.IsNullOrEmpty(rootNamespace))
                {
                    FolderViewModel folder = new FolderViewModel
                    {
                        Name = rootNamespace
                    };
                    root.Items.Add(folder);
                }
                
                AvailableItemsViewModel availableViewModel = new AvailableItemsViewModel
                {
                    AvailableMessages = new ObservableCollection<MessageViewModel>()
                };
                foreach (MessageModel message in model.Messages)
                {
                    MessageViewModel messageViewModel = new MessageViewModel
                    {
                        Name = message.Name,
                        FullName = message.FullName(model),
                        Namespace = message.Namespace.ExtendNamespace(model),
                        ModelId = message.Id
                    };
                    root.AddItem(messageViewModel);
                    availableViewModel.AvailableMessages.Add(messageViewModel);
                }

                foreach (AgentModel agent in model.Agents)
                {
                    root.AddItem(new AgentViewModel
                    {
                        Name = agent.Name,
                        FullName = agent.FullName(model),
                        Namespace = agent.Namespace.ExtendNamespace(model),
                        IncomingEvents = new ObservableCollection<string>(agent.IncomingEvents??Enumerable.Empty<string>()),
                        ProducedEvents = new ObservableCollection<string>(agent.ProducedEvents??Enumerable.Empty<string>()),
                        ConsumingMessages = new ObservableCollection<MessageViewModel>(GenerateMessageMocks(agent.ConsumingMessages)),
                        ProducingMessages = new ObservableCollection<MessageViewModel>(GenerateMessageMocks(agent.ProducedMessages)),
                        AvailableItems = availableViewModel,
                        ModelId = agent.Id
                    });
                }

                return root;
            
                IEnumerable<MessageViewModel> GenerateMessageMocks(string[] agentMessages)
                {
                    List<MessageViewModel> viewModels = new List<MessageViewModel>();
                    foreach (string messageDefinition in agentMessages)
                    {
                        MessageModel message = model.Messages.FirstOrDefault(m => m.FullName(model)
                                                                                   .EndsWith(messageDefinition));
                        viewModels.Add(message != null
                                           ? availableViewModel.AvailableMessages.First(m => m.ModelId == message.Id)
                                           : messageDefinition.GenerateMessageMock(availableViewModel.AvailableMessages));
                    }

                    return viewModels;
                }
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}