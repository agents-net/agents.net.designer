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
        private readonly MessageCollector<AddGeneratorSettingsRequested, ModelUpdated> addGeneratorSettingsCollector;

        public ModelCommandExecuter(IMessageBoard messageBoard) : base(messageBoard)
        {
            addAgentCollector = new MessageCollector<AddAgentRequested, ModelUpdated>(OnMessagesCollected);
            addMessageCollector = new MessageCollector<AddMessageRequested, ModelUpdated>(OnMessagesCollected);
            addGeneratorSettingsCollector = new MessageCollector<AddGeneratorSettingsRequested, ModelUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<AddGeneratorSettingsRequested, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);
            if (set.Message2.Model.GeneratorSettings != null)
            {
                return;
            }

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new GeneratorSettings(), set.Message2.Model,
                                      new GeneratorSettingProperty(), set));
        }

        private void OnMessagesCollected(MessageCollection<AddMessageRequested, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new MessageModel("MessageX"), set.Message2.Model,
                                      new MessagesProperty(), set));
        }

        private void OnMessagesCollected(MessageCollection<AddAgentRequested, ModelUpdated> set)
        {
            set.MarkAsConsumed(set.Message1);

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new AgentModel("AgentX"), set.Message2.Model,
                                      new AgentsProperty(), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            addAgentCollector.TryPush(messageData);
            addMessageCollector.TryPush(messageData);
            addGeneratorSettingsCollector.TryPush(messageData);
        }
    }
}
