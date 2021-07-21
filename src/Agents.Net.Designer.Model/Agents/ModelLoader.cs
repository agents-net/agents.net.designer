using System;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(ModelLoaded))]
    public class ModelLoader : Agent
    {
        public ModelLoader(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            CommunityModel model = new(messages:BuildInMessages());
            OnMessage(new ModelLoaded(model, messageData));
        }

        private MessageModel[] BuildInMessages()
        {
            return new[]
            {
                new MessageModel("InitializeMessage", "Agents.Net", buildIn: true),
                new MessageModel("ExceptionMessage", "Agents.Net", buildIn: true),
                new MessageModel("MessagesAggregated", "Agents.Net", buildIn: true, isGeneric:true, genericParameterCount:1),
            };
        }
    }
}