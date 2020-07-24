using System;
using Agents.Net.Designer.View.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.View.Agents
{
    [Consumes(typeof(GraphViewModelCreated))]
    [Consumes(typeof(MainWindowCreated))]
    [Produces(typeof(GraphViewModelApplied))]
    public class MainWindowDataContextProvider : Agent
    {
        public MainWindowDataContextProvider(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<GraphViewModelCreated, MainWindowCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GraphViewModelCreated, MainWindowCreated> set)
        {
            set.Message2.Window.Dispatcher.Invoke(() =>
            {
                set.Message2.Window.DataContext = set.Message1.ViewModel;
                set.Message2.Window.JsonTextBox.DataContext = set.Message1.ViewModel;
            });
            OnMessage(new GraphViewModelApplied(set.Message1.ViewModel, set));
        }

        private readonly MessageCollector<GraphViewModelCreated, MainWindowCreated> collector;

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
