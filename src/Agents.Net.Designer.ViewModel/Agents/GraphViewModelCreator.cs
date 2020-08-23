using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(GraphViewModelCreated))]
    public class GraphViewModelCreator : Agent
    {        public GraphViewModelCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            OnMessage(new GraphViewModelCreated(new GraphViewModel(), messageData));
        }
    }
}
