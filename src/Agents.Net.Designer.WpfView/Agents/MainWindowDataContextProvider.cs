using Agents.Net.Designer.ViewModel.Messages;
using Agents.Net.Designer.WpfView.Messages;

namespace Agents.Net.Designer.WpfView.Agents
{
    [Consumes(typeof(GraphViewModelCreated))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Consumes(typeof(DetailsViewModelCreated))]
    [Consumes(typeof(SearchViewModelCreated))]
    [Consumes(typeof(MainWindowCreated))]
    public class MainWindowDataContextProvider : Agent
    {
        public MainWindowDataContextProvider(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<GraphViewModelCreated, MainWindowCreated, TreeViewModelCreated, DetailsViewModelCreated, SearchViewModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GraphViewModelCreated, MainWindowCreated, TreeViewModelCreated, DetailsViewModelCreated, SearchViewModelCreated> set)
        {
            set.Message2.Window.Dispatcher.Invoke(() =>
            {
                set.Message2.Window.DataContext = set.Message1.ViewModel;
                set.Message2.Window.TreeView.DataContext = set.Message3.ViewModel;
                set.Message2.Window.DetailsView.DataContext = set.Message4.ViewModel;
                set.Message2.Window.SearchBox.DataContext = set.Message5.ViewModel;
            });
        }

        private readonly MessageCollector<GraphViewModelCreated, MainWindowCreated, TreeViewModelCreated, DetailsViewModelCreated, SearchViewModelCreated> collector;

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
