using System;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(SelectedTreeViewItemChanged))]
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(SelectedModelObjectChanged))]
    public class SelectedTreeItemTranslator : Agent
    {
        private readonly MessageCollector<ModelVersionCreated, SelectedTreeViewItemChanged> collector;

        public SelectedTreeItemTranslator(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<ModelVersionCreated, SelectedTreeViewItemChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModelVersionCreated, SelectedTreeViewItemChanged> set)
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
            
            OnMessage(new SelectedModelObjectChanged(modelObject, set, SelectionSource.Tree));
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
