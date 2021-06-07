using System;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(GraphViewModelUpdated))]
    [Consumes(typeof(TreeViewModelUpdated))]
    [Produces(typeof(SelectedModelObjectChanged))]
    public class NewItemsSelector : Agent
    {
        private readonly MessageCollector<ModifyModel, GraphViewModelUpdated, TreeViewModelUpdated> collector;

        public NewItemsSelector(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ModifyModel, GraphViewModelUpdated, TreeViewModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModifyModel, GraphViewModelUpdated, TreeViewModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.MarkAsConsumed(set.Message2);
            set.MarkAsConsumed(set.Message3);

            if (set.Message1.ModificationType != ModelModification.Add ||
                set.Message1.Target is not CommunityModel ||
                set.Message2.MessageDomain.Root != set.Message1)
            {
                return;
            }
            object selectable = set.Message1.NewValue;
            OnMessage(new SelectedModelObjectChanged(selectable, set, SelectionSource.Internal));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
