using System;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(ModelLoaded))]
    [Produces(typeof(ModelUpdated))]
    public class ModelLoader : Agent
    {
        public ModelLoader(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            CommunityModel model = new CommunityModel(messages:BuildInMessages());
            OnMessage(new ModelLoaded(model, messageData, new ModelUpdated(model, messageData)));
        }

        private MessageModel[] BuildInMessages()
        {
            return new[]
            {
                new MessageModel("InitializeMessage", "Agents.Net", buildIn: true),
                new MessageModel("ExceptionMessage", "Agents.Net", buildIn: true),
            };
        }
    }
}