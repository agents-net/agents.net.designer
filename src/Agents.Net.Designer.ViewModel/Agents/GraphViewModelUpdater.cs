using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(GraphCreated))]
    [Consumes(typeof(GraphViewModelCreated))]
    [Produces(typeof(GraphViewModelUpdated))]
    public class GraphViewModelUpdater : Agent
    {
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