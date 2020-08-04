using System;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(SelectedTreeViewItemChanged))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(SelectedModelObjectChanged))]
    public class SelectedTreeItemTranslator : Agent
    {
        private readonly MessageCollector<ModelUpdated, SelectedTreeViewItemChanged> collector;

        public SelectedTreeItemTranslator(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ModelUpdated, SelectedTreeViewItemChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModelUpdated, SelectedTreeViewItemChanged> set)
        {
            set.MarkAsConsumed(set.Message2);

            object modelObject = null;
            if (set.Message2.SelectedItem is AgentViewModel agent)
            {
                modelObject = set.Message1.Model.Agents.First(a => a.Id == agent.ModelId);
            }
            else if (set.Message2.SelectedItem is MessageViewModel message)
            {
                modelObject = set.Message1.Model.Messages.First(a => a.Id == message.ModelId);
            }
            
            OnMessage(new SelectedModelObjectChanged(modelObject, set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
