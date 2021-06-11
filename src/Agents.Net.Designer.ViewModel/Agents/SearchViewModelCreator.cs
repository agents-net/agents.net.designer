using System;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(SearchViewModelCreated))]
    public class SearchViewModelCreator : Agent
    {
        public SearchViewModelCreator(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            OnMessage(new SearchViewModelCreated(messageData, new SearchViewModel()));
        }
    }
}