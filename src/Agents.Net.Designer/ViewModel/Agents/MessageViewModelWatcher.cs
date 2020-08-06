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
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModifyModel))]
    public class MessageViewModelWatcher : Agent, IDisposable
    {
        private Message changedMessage;
        private MessageViewModel viewModel;
        private CommunityModel latestModel;

        public MessageViewModelWatcher(IMessageBoard messageBoard) : base(messageBoard)
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
            if (!(viewModelChanged.SelectedItem is MessageViewModel messageViewModel))
            {
                return;
            }

            MessageViewModel oldViewModel = Interlocked.Exchange(ref viewModel, messageViewModel);
            if (oldViewModel != null)
            {
                oldViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
            changedMessage = messageData;
            messageViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            MessageModel oldModel = latestModel.Messages.First(a => a.Id == viewModel.ModelId);
            switch (e.PropertyName)
            {
                case nameof(MessageViewModel.Name):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Name,
                                              viewModel.Name,
                                              oldModel,
                                              new MessageNameProperty(), 
                                              changedMessage));
                    break;
                case nameof(MessageViewModel.RelativeNamespace):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Namespace,
                                              viewModel.RelativeNamespace,
                                              oldModel,
                                              new MessageNamespaceProperty(), 
                                              changedMessage));
                    break;
                case nameof(MessageViewModel.DecoratedMessage):
                    MessageDecoratorModel oldDecoratorModel = (MessageDecoratorModel) oldModel;
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldDecoratorModel.DecoratedMessage,
                                              viewModel.DecoratedMessage.ModelId,
                                              oldModel,
                                              new MessageDecoratorDecoratedMessageProperty(), 
                                              changedMessage));
                    break;
                case nameof(MessageViewModel.MessageType):
                    SwitchMessageType(oldModel);
                    break;
            }
        }

        private void SwitchMessageType(MessageModel oldModel)
        {
            MessageModel newModel;
            switch (viewModel.MessageType)
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
                    throw new InvalidOperationException($"Message type {viewModel.MessageType} is not known.");
            }

            OnMessage(new ModifyModel(ModelModification.Change, oldModel, newModel,
                                      oldModel.ContainingPackage, new PackageMessagesProperty(), changedMessage));
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
