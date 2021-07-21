#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using Agents.Net.Designer.ViewModel.Messages;
using Agents.Net.Designer.WpfView.Messages;

namespace Agents.Net.Designer.WpfView.Agents
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