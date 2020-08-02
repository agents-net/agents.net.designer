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
                case nameof(MessageViewModel.Namespace):
                    OnMessage(new ModifyModel(ModelModification.Change,
                                              oldModel.Namespace,
                                              viewModel.Namespace,
                                              oldModel,
                                              new MessageNamespaceProperty(), 
                                              changedMessage));
                    break;
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
