using System;
using Agents.Net.Designer.View.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.View.Agents
{
    [Consumes(typeof(GraphViewModelCreated))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Consumes(typeof(DetailsViewModelCreated))]
    [Consumes(typeof(MainWindowCreated))]
    public class MainWindowDataContextProvider : Agent
    {
        public MainWindowDataContextProvider(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<GraphViewModelCreated, MainWindowCreated, TreeViewModelCreated, DetailsViewModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GraphViewModelCreated, MainWindowCreated, TreeViewModelCreated, DetailsViewModelCreated> set)
        {
            set.Message2.Window.Dispatcher.Invoke(() =>
            {
                set.Message2.Window.DataContext = set.Message1.ViewModel;
                set.Message2.Window.TreeView.DataContext = set.Message3.ViewModel;
                set.Message2.Window.DetailsView.DataContext = set.Message4.ViewModel;
            });
        }

        private readonly MessageCollector<GraphViewModelCreated, MainWindowCreated, TreeViewModelCreated, DetailsViewModelCreated> collector;

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
