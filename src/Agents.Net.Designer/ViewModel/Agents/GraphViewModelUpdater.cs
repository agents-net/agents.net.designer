using System;
using Agents.Net;
using Agents.Net.Designer.MicrosoftGraph.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(GraphCreated))]
    [Consumes(typeof(GraphViewModelCreated))]
    [Produces(typeof(GraphViewModelUpdated))]
    public class GraphViewModelUpdater : Agent
    {        public GraphViewModelUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<GraphCreated, GraphViewModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GraphCreated, GraphViewModelCreated> set)
        {
            set.Message2.ViewModel.Graph = set.Message1.Graph;
        }

        private readonly MessageCollector<GraphCreated, GraphViewModelCreated> collector;

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
