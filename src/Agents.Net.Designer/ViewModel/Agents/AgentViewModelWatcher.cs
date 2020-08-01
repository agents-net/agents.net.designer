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
            agentViewModel.DeleteItemRequested += ViewModelOnDeleteItemRequested;
        }

        private void ViewModelOnDeleteItemRequested(object? sender, DeleteItemEventArgs e)
        {
            AgentModel oldModel = latestModel.Agents.First(a => a.Id == viewModel.ModelId);
            switch (e.TargetProperty)
            {
                case nameof(AgentViewModel.ConsumingMessages):
                {
                    MessageViewModel message = e.DeletedItem.AssertTypeOf<MessageViewModel>();
                    object messageId = message.ModelId != default ? (object) message.ModelId : message.FullName;
                    OnMessage(new ModifyModel(ModelModification.Remove,
                                              messageId,
                                              null,
                                              oldModel,
                                              new AgentConsumingMessagesProperty(),
                                              changedMessage));
                    break;
                }
                case nameof(AgentViewModel.ProducingMessages):
                {
                    MessageViewModel message = e.DeletedItem.AssertTypeOf<MessageViewModel>();
                    object messageId = message.ModelId != default ? (object) message.ModelId : message.FullName;
                    OnMessage(new ModifyModel(ModelModification.Remove,
                                              messageId,
                                              null,
                                              oldModel,
                                              new AgentProducedMessagesProperty(), 
                                              changedMessage));
                    break;
                }
                case nameof(AgentViewModel.IncomingEvents):
                    OnMessage(new ModifyModel(ModelModification.Remove,
                                              e.DeletedItem.AssertTypeOf<string>(),
                                              null,
                                              oldModel,
                                              new AgentIncomingEventsProperty(), 
                                              changedMessage));
                    break;
                case nameof(AgentViewModel.ProducedEvents):
                    OnMessage(new ModifyModel(ModelModification.Remove,
                                              e.DeletedItem.AssertTypeOf<string>(),
                                              null,
                                              oldModel,
                                              new AgentProducedEventsProperty(), 
                                              changedMessage));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown deleted item {e.TargetProperty}");
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AgentModel oldModel = latestModel.Agents.First(a => a.Id == viewModel.ModelId);
            switch (e.PropertyName)
            {
                case nameof(AgentViewModel.Name):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Name,
                                              viewModel.Name,
                                              oldModel,
                                              new AgentNameProperty(),
                                              changedMessage));
                    break;
                case nameof(AgentViewModel.Namespace):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Namespace,
                                              viewModel.Namespace,
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
                    AddEvent(viewModel.NewIncomingEvent, new AgentIncomingEventsProperty());
                    viewModel.NewIncomingEvent = string.Empty;
                    break;
                case nameof(AgentViewModel.NewProducedEvent):
                    AddEvent(viewModel.NewProducedEvent, new AgentProducedEventsProperty());
                    viewModel.NewProducedEvent = string.Empty;
                    break;
            }

            void AddConsumingMessage()
            {
                if (string.IsNullOrEmpty(viewModel.NewConsumingMessage))
                {
                    return;
                }

                MessageViewModel selectedConsumingViewModel = viewModel.NewConsumingMessageObject as MessageViewModel;
                OnMessage(new ModifyModel(ModelModification.Add,
                                          null,
                                          selectedConsumingViewModel != null
                                              ? selectedConsumingViewModel.ModelId != default
                                                    ? (object) selectedConsumingViewModel.ModelId
                                                    : selectedConsumingViewModel.FullName
                                              : viewModel.NewConsumingMessage,
                                          oldModel,
                                          new AgentConsumingMessagesProperty(),
                                          changedMessage));
                viewModel.NewConsumingMessage = string.Empty;
            }

            void AddProducedMessage()
            {
                if (string.IsNullOrEmpty(viewModel.NewProducingMessage))
                {
                    return;
                }

                MessageViewModel selectedProducingViewModel = viewModel.NewProducingMessageObject as MessageViewModel;
                OnMessage(new ModifyModel(ModelModification.Add,
                                          null,
                                          selectedProducingViewModel != null
                                              ? selectedProducingViewModel.ModelId != default
                                                    ? (object) selectedProducingViewModel.ModelId
                                                    : selectedProducingViewModel.FullName
                                              : viewModel.NewProducingMessage,
                                          oldModel,
                                          new AgentProducedMessagesProperty(),
                                          changedMessage));
                viewModel.NewProducingMessage = string.Empty;
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
                viewModel.DeleteItemRequested -= ViewModelOnDeleteItemRequested;
            }
        }
    }
}