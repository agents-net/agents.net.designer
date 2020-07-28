using System;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(SelectedModelObjectChanged))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Produces(typeof(ViewModelChangeApplying))]
    public class TreeItemSelector : Agent
    {
        private readonly MessageCollector<TreeViewModelCreated, SelectedModelObjectChanged> collector;
        public TreeItemSelector(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<TreeViewModelCreated, SelectedModelObjectChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TreeViewModelCreated, SelectedModelObjectChanged> set)
        {
            set.MarkAsConsumed(set.Message2);
            if (set.Message2.SelectedObject is AgentModel agentModel)
            {
                TreeViewItem viewItem = set.Message1.ViewModel.Community.FindViewItemById(agentModel.Id);
                OnMessage(new ViewModelChangeApplying(() => viewItem.IsSelected = true, set));
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}