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
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(ModifyModel))]
    public class AgentViewModelWatcher : Agent
    {
        private Tuple<AgentViewModel, Message[], CommunityModel> latestData;
        private readonly MessageCollector<ModelVersionCreated, SelectedTreeViewItemChanged> collector;
        
        public AgentViewModelWatcher(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<ModelVersionCreated, SelectedTreeViewItemChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModelVersionCreated, SelectedTreeViewItemChanged> set)
        {
            if (set.Message2.SelectedItem is not AgentViewModel agentViewModel)
            {
                return;
            }
            Tuple<AgentViewModel, Message[], CommunityModel> oldData = Interlocked.Exchange(ref latestData, new Tuple<AgentViewModel, Message[], CommunityModel>(agentViewModel, set.ToArray(), set.Message1.Model));
            
            if (oldData?.Item1 != null)
            {
                oldData.Item1.PropertyChanged -= ViewModelOnPropertyChanged;
                oldData.Item1.DeleteItemRequested -= ViewModelOnDeleteItemRequested;
            }
            agentViewModel.PropertyChanged += ViewModelOnPropertyChanged;
            agentViewModel.DeleteItemRequested += ViewModelOnDeleteItemRequested;
        }
        
        

        private void ViewModelOnDeleteItemRequested(object? sender, DeleteItemEventArgs e)
        {
            AgentModel oldModel = latestData.Item3.Agents.First(a => a.Id == latestData.Item1.ModelId);
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
                                              latestData.Item2));
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
                                              latestData.Item2));
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
                                              latestData.Item2));
                    break;
                }
                case nameof(AgentViewModel.IncomingEvents):
                    OnMessage(new ModifyModel(ModelModification.Remove,
                                              e.DeletedItem.AssertTypeOf<string>(),
                                              null,
                                              oldModel,
                                              new AgentIncomingEventsProperty(), 
                                              latestData.Item2));
                    break;
                case nameof(AgentViewModel.ProducedEvents):
                    OnMessage(new ModifyModel(ModelModification.Remove,
                                              e.DeletedItem.AssertTypeOf<string>(),
                                              null,
                                              oldModel,
                                              new AgentProducedEventsProperty(), 
                                              latestData.Item2));
                    break;
                default:
                    throw new InvalidOperationException($"Unknown deleted item {e.TargetProperty}");
            }
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AgentModel oldModel = latestData.Item3.Agents.First(a => a.Id == latestData.Item1.ModelId);
            switch (e.PropertyName)
            {
                case nameof(AgentViewModel.Name):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Name,
                                              latestData.Item1.Name,
                                              oldModel,
                                              new AgentNameProperty(),
                                              latestData.Item2));
                    break;
                case nameof(AgentViewModel.RelativeNamespace):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Namespace,
                                              latestData.Item1.RelativeNamespace,
                                              oldModel,
                                              new AgentNamespaceProperty(),
                                              latestData.Item2));
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
                    AddEvent(latestData.Item1.NewIncomingEvent, new AgentIncomingEventsProperty(), oldModel);
                    latestData.Item1.NewIncomingEvent = string.Empty;
                    break;
                case nameof(AgentViewModel.NewProducedEvent):
                    AddEvent(latestData.Item1.NewProducedEvent, new AgentProducedEventsProperty(), oldModel);
                    latestData.Item1.NewProducedEvent = string.Empty;
                    break;
                case nameof(AgentViewModel.AgentType):
                    SwitchAgentType(oldModel);
                    break;
            }
        }

        private void SwitchAgentType(AgentModel oldModel)
        {
            AgentModel newModel;
            switch (latestData.Item1.AgentType)
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
                    throw new InvalidOperationException($"Agent type {latestData.Item1.AgentType} is not known.");
            }

            OnMessage(new ModifyModel(ModelModification.Change, oldModel, newModel,
                                      oldModel.ContainingPackage, new PackageAgentsProperty(), latestData.Item2));
        }

        private void AddEvent(string @event, PropertySpecifier propertySpecifier, AgentModel oldModel)
        {
            if (string.IsNullOrEmpty(@event))
            {
                return;
            }

            OnMessage(new ModifyModel(ModelModification.Add, null, @event,
                                      oldModel, propertySpecifier, latestData.Item2));
        }

        private void AddProducedMessage(AgentModel oldModel)
        {
            if (string.IsNullOrEmpty(latestData.Item1.NewProducingMessage))
            {
                return;
            }

            MessageViewModel selectedProducingViewModel = latestData.Item1.NewProducingMessageObject as MessageViewModel;
            OnMessage(new ModifyModel(ModelModification.Add, null, selectedProducingViewModel != null
                                                                       ? (object) selectedProducingViewModel.ModelId
                                                                       : latestData.Item1.NewProducingMessage, oldModel, new AgentProducedMessagesProperty(), latestData.Item2));
            latestData.Item1.NewProducingMessage = string.Empty;
        }

        private void AddConsumingMessage(AgentModel oldModel)
        {
            if (string.IsNullOrEmpty(latestData.Item1.NewConsumingMessage))
            {
                return;
            }

            MessageViewModel selectedConsumingViewModel = latestData.Item1.NewConsumingMessageObject as MessageViewModel;
            OnMessage(new ModifyModel(ModelModification.Add, null, selectedConsumingViewModel != null
                                                                       ? (object) selectedConsumingViewModel.ModelId
                                                                       : latestData.Item1.NewConsumingMessage, oldModel, new AgentConsumingMessagesProperty(), latestData.Item2));
            latestData.Item1.NewConsumingMessage = string.Empty;
        }

        private void AddInterceptingMessage(AgentModel oldModel)
        {
            if (string.IsNullOrEmpty(latestData.Item1.NewInterceptingMessage) ||
                oldModel is not InterceptorAgentModel)
            {
                return;
            }

            MessageViewModel selectedInterceptingViewModel = latestData.Item1.NewInterceptingMessageObject as MessageViewModel;
            OnMessage(new ModifyModel(ModelModification.Add, null, selectedInterceptingViewModel != null
                                                                       ? (object) selectedInterceptingViewModel.ModelId
                                                                       : latestData.Item1.NewInterceptingMessage, oldModel, 
                                      new InterceptorAgentInterceptingMessagesProperty(), latestData.Item2));
            latestData.Item1.NewInterceptingMessage = string.Empty;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (latestData != null)
                {
                    latestData.Item1.PropertyChanged -= ViewModelOnPropertyChanged;
                    latestData.Item1.DeleteItemRequested -= ViewModelOnDeleteItemRequested;
                }
            }
            base.Dispose(disposing);
        }
    }
}