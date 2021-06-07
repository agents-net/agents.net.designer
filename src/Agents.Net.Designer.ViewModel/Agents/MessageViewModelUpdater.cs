using System;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Produces(typeof(ViewModelChangeApplying))]
    [Produces(typeof(TreeViewModelUpdated))]
    public class MessageViewModelUpdater : Agent
    {
        private readonly MessageCollector<TreeViewModelCreated, ModifyModel> collector;

        public MessageViewModelUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<TreeViewModelCreated, ModifyModel>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TreeViewModelCreated, ModifyModel> set)
        {
            set.MarkAsConsumed(set.Message2);
            if (set.Message2.Target is not MessageModel oldModel)
            {
                return;
            }

            MessageViewModel changingViewModel = (MessageViewModel) set.Message1.ViewModel.Community.FindViewItemById(oldModel.Id);
            OnMessage(new ViewModelChangeApplying(() =>
            {
                switch (set.Message2.Property)
                {
                    case MessageNameProperty _:
                        changingViewModel.Name = set.Message2.NewValue.AssertTypeOf<string>();
                        string fullNamespace = changingViewModel.FullName.Substring(0,changingViewModel.FullName.Length-set.Message2.OldValue.AssertTypeOf<string>().Length+1);
                        changingViewModel.FullName =  $"{fullNamespace}.{changingViewModel.Name}";
                        break;
                    case MessageNamespaceProperty _:
                        changingViewModel.RelativeNamespace = set.Message2.NewValue.AssertTypeOf<string>();
                        changingViewModel.FullName = $"{set.Message2.NewValue.AssertTypeOf<string>().ExtendNamespace(oldModel)}.{changingViewModel.Name}";
                        RestructureViewModel(changingViewModel, set.Message1.ViewModel);
                        break;
                    case MessageDecoratorDecoratedMessageProperty _:
                        changingViewModel.DecoratedMessage =  changingViewModel.AvailableItems
                                                                               .AvailableMessages
                                                                               .FirstOrDefault(m => m.ModelId == 
                                                                                                    set.Message2.NewValue.AssertTypeOf<Guid>());
                        break;
                }
                OnMessage(new TreeViewModelUpdated(set));
            }, set));
        }

        private void RestructureViewModel(MessageViewModel changingViewModel, TreeViewModel viewModel)
        {
            viewModel.Community.RemoveItem(changingViewModel);
            viewModel.Community.AddItem(changingViewModel);
            changingViewModel.IsSelected = true;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
