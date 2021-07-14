using System;
using System.Linq;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(TreeViewModelCreated))]
    [Consumes(typeof(TreeViewModelUpdated))]
    [Consumes(typeof(SearchViewModelCreated))]
    public class SearchSourceUpdater : Agent
    {
        private readonly MessageCollector<SearchViewModelCreated, TreeViewModelCreated, TreeViewModelUpdated> collector;
        
        public SearchSourceUpdater(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<SearchViewModelCreated, TreeViewModelCreated, TreeViewModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<SearchViewModelCreated, TreeViewModelCreated, TreeViewModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message3);

            AvailableItemsViewModel searchSource = set.Message2.ViewModel.Community.FindItemsByType<MessageViewModel>()
                                                      .Select(m => m.AvailableItems)
                                                      .FirstOrDefault(a => a != null)
                                                   ?? set.Message2.ViewModel.Community.FindItemsByType<AgentViewModel>()
                                                         .Select(m => m.AvailableItems)
                                                         .FirstOrDefault(a => a != null);
            if (searchSource != null)
            {
                set.Message1.ViewModel.SetSearchSource(searchSource);
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}