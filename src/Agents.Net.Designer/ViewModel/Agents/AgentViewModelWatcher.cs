using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(SelectedTreeViewItemChanged))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModifyModel))]
    public class AgentViewModelWatcher : Agent, IDisposable
    {
        private Message changedMessage;
        private AgentViewModel viewModel;
        private CommunityModel latestModel;
        
        public AgentViewModelWatcher(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out ModelUpdated modelUpdated))
            {
                latestModel = modelUpdated.Model;
                return;
            }
            SelectedTreeViewItemChanged viewModelChanged = messageData.Get<SelectedTreeViewItemChanged>();
            if (!(viewModelChanged.SelectedItem is AgentViewModel agentViewModel))
            {
                return;
            }

            AgentViewModel oldViewModel = Interlocked.Exchange(ref viewModel, agentViewModel);
            if (oldViewModel != null)
            {
                oldViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
            changedMessage = messageData;
            agentViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AgentViewModel agentViewModel = (AgentViewModel) sender;
            AgentModel oldModel = latestModel.Agents.First(a => a.Id == agentViewModel.ModelId);
            switch (e.PropertyName)
            {
                case nameof(AgentViewModel.Name):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Name,
                                              agentViewModel.Name,
                                              oldModel,
                                              new AgentNameProperty(),
                                              changedMessage));
                    break;
                case nameof(AgentViewModel.Namespace):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Namespace,
                                              agentViewModel.Namespace,
                                              oldModel,
                                              new AgentNamespaceProperty(),
                                              changedMessage));
                    break;
                case nameof(AgentViewModel.NewConsumingMessage):
                    AddConsumingMessage();
                    break;
                case nameof(AgentViewModel.NewProducingMessage):
                    AddProducedMessage();
                    break;
                case nameof(AgentViewModel.NewIncomingEvent):
                    AddEvent(agentViewModel.NewIncomingEvent, new AgentIncomingEventsProperty());
                    agentViewModel.NewIncomingEvent = string.Empty;
                    break;
                case nameof(AgentViewModel.NewProducedEvent):
                    AddEvent(agentViewModel.NewProducedEvent, new AgentProducedEventsProperty());
                    agentViewModel.NewProducedEvent = string.Empty;
                    break;
            }

            void AddConsumingMessage()
            {
                if (string.IsNullOrEmpty(agentViewModel.NewConsumingMessage))
                {
                    return;
                }

                MessageViewModel selectedConsumingViewModel = agentViewModel.NewConsumingMessageObject as MessageViewModel;
                OnMessage(new ModifyModel(ModelModification.Add,
                                          null,
                                          selectedConsumingViewModel != null
                                              ? selectedConsumingViewModel.ModelId != default
                                                    ? (object) selectedConsumingViewModel.ModelId
                                                    : selectedConsumingViewModel.FullName
                                              : agentViewModel.NewConsumingMessage,
                                          oldModel,
                                          new AgentConsumingMessagesProperty(),
                                          changedMessage));
                agentViewModel.NewConsumingMessage = string.Empty;
            }

            void AddProducedMessage()
            {
                if (string.IsNullOrEmpty(agentViewModel.NewProducingMessage))
                {
                    return;
                }

                MessageViewModel selectedProducingViewModel = agentViewModel.NewProducingMessageObject as MessageViewModel;
                OnMessage(new ModifyModel(ModelModification.Add,
                                          null,
                                          selectedProducingViewModel != null
                                              ? selectedProducingViewModel.ModelId != default
                                                    ? (object) selectedProducingViewModel.ModelId
                                                    : selectedProducingViewModel.FullName
                                              : agentViewModel.NewProducingMessage,
                                          oldModel,
                                          new AgentProducedMessagesProperty(),
                                          changedMessage));
                agentViewModel.NewProducingMessage = string.Empty;
            }

            void AddEvent(string @event, PropertySpecifier propertySpecifier)
            {
                if (string.IsNullOrEmpty(@event))
                {
                    return;
                }

                OnMessage(new ModifyModel(ModelModification.Add,
                                          null,
                                          @event,
                                          oldModel,
                                          propertySpecifier,
                                          changedMessage));
            }
        }

        public void Dispose()
        {
            if (viewModel != null)
            {
                viewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
        }
    }
}