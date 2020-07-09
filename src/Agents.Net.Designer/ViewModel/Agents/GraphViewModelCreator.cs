using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    public class GraphViewModelCreator : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition GraphViewModelCreatorDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      InitializeMessage.InitializeMessageDefinition
                                  },
                                  new []
                                  {
                                      GraphViewModelCreated.GraphViewModelCreatedDefinition
                                  });

        #endregion

        public GraphViewModelCreator(IMessageBoard messageBoard) : base(GraphViewModelCreatorDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            OnMessage(new GraphViewModelCreated(new GraphViewModel(), messageData));
        }
    }
}
