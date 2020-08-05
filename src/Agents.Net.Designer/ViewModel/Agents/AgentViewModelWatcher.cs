using System;
using System.Collections.ObjectModel;
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
                case nameof(AgentViewModel.InterceptingMessages):
                {
                    MessageViewModel message = e.DeletedItem.AssertTypeOf<MessageViewModel>();
                    object messageId = message.ModelId != default ? (object) message.ModelId : message.FullName;
                    OnMessage(new ModifyModel(ModelModification.Remove,
                                              messageId,
                                              null,
                                              oldModel,
                                              new InterceptorAgentInterceptingMessagesProperty(), 
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
                case nameof(AgentViewModel.RelativeNamespace):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Namespace,
                                              viewModel.RelativeNamespace,
                                              oldModel,
                                              new AgentNamespaceProperty(),
                                              changedMessage));
                    break;
                case nameof(AgentViewModel.NewConsumingMessage):
                    AddConsumingMessage(oldModel);
                    break;
                case nameof(AgentViewModel.NewProducingMessage):
                    AddProducedMessage(oldModel);
                    break;
                case nameof(AgentViewModel.NewInterceptingMessage):
                    AddInterceptingMessage(oldModel);
                    break;
                case nameof(AgentViewModel.NewIncomingEvent):
                    AddEvent(viewModel.NewIncomingEvent, new AgentIncomingEventsProperty(), oldModel);
                    viewModel.NewIncomingEvent = string.Empty;
                    break;
                case nameof(AgentViewModel.NewProducedEvent):
                    AddEvent(viewModel.NewProducedEvent, new AgentProducedEventsProperty(), oldModel);
                    viewModel.NewProducedEvent = string.Empty;
                    break;
                case nameof(AgentViewModel.AgentType):
                    SwitchAgentType(oldModel);
                    break;
            }
        }

        private void SwitchAgentType(AgentModel oldModel)
        {
            AgentModel newModel;
            switch (viewModel.AgentType)
            {
                case AgentType.Agent:
                    newModel = new AgentModel(oldModel.Name, oldModel.Namespace, oldModel.ConsumingMessages,
                                              oldModel.ProducedMessages, oldModel.IncomingEvents,
                                              oldModel.ProducedEvents,
                                              oldModel.Id);
                    break;
                case AgentType.Interceptor:
                    newModel = new InterceptorAgentModel(oldModel.Name, oldModel.Namespace, oldModel.ConsumingMessages,
                                                         oldModel.ProducedMessages, oldModel.IncomingEvents,
                                                         oldModel.ProducedEvents,
                                                         oldModel.Id);
                    break;
                default:
                    throw new InvalidOperationException($"Agent type {viewModel.AgentType} is not known.");
            }

            OnMessage(new ModifyModel(ModelModification.Change, oldModel, newModel,
                                      oldModel.ContainingPackage, new PackageAgentsProperty(), changedMessage));
        }

        private void AddEvent(string @event, PropertySpecifier propertySpecifier, AgentModel oldModel)
        {
            if (string.IsNullOrEmpty(@event))
            {
                return;
            }

            OnMessage(new ModifyModel(ModelModification.Add, null, @event,
                                      oldModel, propertySpecifier, changedMessage));
        }

        private void AddProducedMessage(AgentModel oldModel)
        {
            if (string.IsNullOrEmpty(viewModel.NewProducingMessage))
            {
                return;
            }

            MessageViewModel selectedProducingViewModel = viewModel.NewProducingMessageObject as MessageViewModel;
            OnMessage(new ModifyModel(ModelModification.Add, null, selectedProducingViewModel != null
                                                                       ? (object) selectedProducingViewModel.ModelId
                                                                       : viewModel.NewProducingMessage, oldModel, new AgentProducedMessagesProperty(), changedMessage));
            viewModel.NewProducingMessage = string.Empty;
        }

        private void AddConsumingMessage(AgentModel oldModel)
        {
            if (string.IsNullOrEmpty(viewModel.NewConsumingMessage))
            {
                return;
            }

            MessageViewModel selectedConsumingViewModel = viewModel.NewConsumingMessageObject as MessageViewModel;
            OnMessage(new ModifyModel(ModelModification.Add, null, selectedConsumingViewModel != null
                                                                       ? (object) selectedConsumingViewModel.ModelId
                                                                       : viewModel.NewConsumingMessage, oldModel, new AgentConsumingMessagesProperty(), changedMessage));
            viewModel.NewConsumingMessage = string.Empty;
        }

        private void AddInterceptingMessage(AgentModel oldModel)
        {
            if (string.IsNullOrEmpty(viewModel.NewInterceptingMessage) ||
                !(oldModel is InterceptorAgentModel))
            {
                return;
            }

            MessageViewModel selectedInterceptingViewModel = viewModel.NewInterceptingMessageObject as MessageViewModel;
            OnMessage(new ModifyModel(ModelModification.Add, null, selectedInterceptingViewModel != null
                                                                       ? (object) selectedInterceptingViewModel.ModelId
                                                                       : viewModel.NewInterceptingMessage, oldModel, 
                                      new InterceptorAgentInterceptingMessagesProperty(), changedMessage));
            viewModel.NewInterceptingMessage = string.Empty;
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