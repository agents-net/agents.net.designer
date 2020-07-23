using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(JsonViewModelCreated))]
    public class JsonViewModelCreator : Agent
    {        public JsonViewModelCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            OnMessage(new JsonViewModelCreated(new JsonViewModel(), messageData));
        }
    }
}
