using System;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Intercepts(typeof(ModifyModel))]
    [Consumes(typeof(GraphViewModelUpdated))]
    [Consumes(typeof(TreeViewModelUpdated))]
    [Produces(typeof(SelectedModelObjectChanged))]
    public class NewItemsSelector : InterceptorAgent
    {
        private readonly MessageCollector<GraphViewModelUpdated, TreeViewModelUpdated> collector;

        public NewItemsSelector(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<GraphViewModelUpdated, TreeViewModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GraphViewModelUpdated, TreeViewModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.MarkAsConsumed(set.Message2);

            object selectable = objectToSelect;
            if (selectable != null)
            {
                OnMessage(new SelectedModelObjectChanged(selectable, set));
            }
        }

        private object objectToSelect;

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            ModifyModel modifyModel = messageData.Get<ModifyModel>();
            objectToSelect = modifyModel.ModificationType != ModelModification.Add ||
                             !(modifyModel.Target is CommunityModel)
                                 ? null
                                 : modifyModel.NewValue;

            return InterceptionAction.Continue;
        }
    }
}
