using System;
using Agents.Net.Designer.View.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.View.Agents
{
    [Consumes(typeof(ViewModelChangeApplying))]
    [Consumes(typeof(MainWindowCreated))]
    public class ViewModelChangeApplier : Agent
    {
        private readonly MessageCollector<ViewModelChangeApplying, MainWindowCreated> collector;
        public ViewModelChangeApplier(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<ViewModelChangeApplying, MainWindowCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ViewModelChangeApplying, MainWindowCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.Message2.Window.Dispatcher.Invoke(set.Message1.ChangeAction);
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}