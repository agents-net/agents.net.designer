using System;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(SelectedTreeViewItemChanged))]
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(ModifyModel))]
    public class MessageViewModelWatcher : Agent
    {
        private Tuple<MessageViewModel, Message[], CommunityModel> latestData;
        private readonly MessageCollector<ModelVersionCreated, SelectedTreeViewItemChanged> collector;

        public MessageViewModelWatcher(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ModelVersionCreated, SelectedTreeViewItemChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModelVersionCreated, SelectedTreeViewItemChanged> set)
        {
            if (set.Message2.SelectedItem is not MessageViewModel messageViewModel)
            {
                return;
            }
            Tuple<MessageViewModel, Message[], CommunityModel> oldData = Interlocked.Exchange(ref latestData, new Tuple<MessageViewModel, Message[], CommunityModel>(messageViewModel, set.ToArray(), set.Message1.Model));
            if (oldData?.Item1 != null)
            {
                oldData.Item1.PropertyChanged -= ViewModelOnPropertyChanged;
            }
            messageViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MessageModel oldModel = latestData.Item3.Messages.First(a => a.Id == latestData.Item1.ModelId);
            switch (e.PropertyName)
            {
                case nameof(MessageViewModel.Name):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Name,
                                              latestData.Item1.Name,
                                              oldModel,
                                              new MessageNameProperty(), 
                                              latestData.Item2));
                    break;
                case nameof(MessageViewModel.RelativeNamespace):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Namespace,
                                              latestData.Item1.RelativeNamespace,
                                              oldModel,
                                              new MessageNamespaceProperty(), 
                                              latestData.Item2));
                    break;
                case nameof(MessageViewModel.DecoratedMessage):
                    MessageDecoratorModel oldDecoratorModel = (MessageDecoratorModel) oldModel;
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldDecoratorModel.DecoratedMessage,
                                              latestData.Item1.DecoratedMessage.ModelId,
                                              oldModel,
                                              new MessageDecoratorDecoratedMessageProperty(), 
                                              latestData.Item2));
                    break;
                case nameof(MessageViewModel.MessageType):
                    SwitchMessageType(oldModel);
                    break;
            }
        }

        private void SwitchMessageType(MessageModel oldModel)
        {
            MessageModel newModel;
            switch (latestData.Item1.MessageType)
            {
                case MessageType.Message:
                    newModel = new MessageModel(oldModel.Name, oldModel.Namespace, oldModel.Id,
                                                oldModel.BuildIn);
                    break;
                case MessageType.MessageDecorator:
                    newModel = new MessageDecoratorModel(oldModel.Name, oldModel.Namespace, oldModel.Id,
                                                         oldModel.BuildIn);
                    break;
                default:
                    throw new InvalidOperationException($"Message type {latestData.Item1.MessageType} is not known.");
            }

            OnMessage(new ModifyModel(ModelModification.Change, oldModel, newModel,
                                      oldModel.ContainingPackage, new PackageMessagesProperty(), latestData.Item2));
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && latestData != null)
            {
                latestData.Item1.PropertyChanged -= ViewModelOnPropertyChanged;
            }
            base.Dispose(disposing);
        }
    }
}
