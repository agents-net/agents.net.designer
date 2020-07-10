using System;
using Agents.Net;
using Agents.Net.Designer.MicrosoftGraph.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    public class GraphViewModelUpdater : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition GraphViewModelUpdaterDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      GraphCreated.GraphCreatedDefinition,
                                      GraphViewModelCreated.GraphViewModelCreatedDefinition
                                  },
                                  Array.Empty<MessageDefinition>());

        #endregion

        public GraphViewModelUpdater(IMessageBoard messageBoard) : base(GraphViewModelUpdaterDefinition, messageBoard)
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
