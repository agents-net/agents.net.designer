using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(AddAgentRequested))]
    [Consumes(typeof(AddMessageRequested))]
    [Consumes(typeof(AddGeneratorSettingsRequested))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModifyModel))]
    public class ModelCommandExecuter : Agent
    {
        private readonly MessageCollector<AddAgentRequested, ModelUpdated> addAgentCollector;
        private readonly MessageCollector<AddMessageRequested, ModelUpdated> addMessageCollector;

        public ModelCommandExecuter(IMessageBoard messageBoard) : base(messageBoard)
        {
            addAgentCollector = new MessageCollector<AddAgentRequested, ModelUpdated>(OnMessagesCollected);
            addMessageCollector = new MessageCollector<AddMessageRequested, ModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<AddMessageRequested, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new MessageModel("MessageX"), set.Message2.Model,
                                      new PackageMessagesProperty(), set));
        }

        private void OnMessagesCollected(MessageCollection<AddAgentRequested, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new AgentModel("AgentX"), set.Message2.Model,
                                      new PackageAgentsProperty(), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            addAgentCollector.TryPush(messageData);
            addMessageCollector.TryPush(messageData);
        }
    }
}
