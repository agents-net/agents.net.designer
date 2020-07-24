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
    [Consumes(typeof(ModelCreated))]
    [Produces(typeof(ModelUpdated))]
    public class CommandModelUpdater : Agent
    {
        private readonly MessageCollector<AddAgentRequested, ModelCreated> addAgentCollector;
        private readonly MessageCollector<AddMessageRequested, ModelCreated> addMessageCollector;
        private readonly MessageCollector<AddGeneratorSettingsRequested, ModelCreated> addGeneratorSettingsCollector;

        public CommandModelUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            addAgentCollector = new MessageCollector<AddAgentRequested, ModelCreated>(OnMessagesCollected);
            addMessageCollector = new MessageCollector<AddMessageRequested, ModelCreated>(OnMessagesCollected);
            addGeneratorSettingsCollector = new MessageCollector<AddGeneratorSettingsRequested, ModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<AddGeneratorSettingsRequested, ModelCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            if (set.Message2.Model.GeneratorSettings != null)
            {
                return;
            }

            CommunityModel updatedModel = set.Message2.Model.Clone();
            updatedModel.GeneratorSettings = new GeneratorSettings();
            OnMessage(new ModelUpdated(updatedModel,set));
        }

        private void OnMessagesCollected(MessageCollection<AddMessageRequested, ModelCreated> set)
        {
            set.MarkAsConsumed(set.Message1);

            CommunityModel updatedModel = set.Message2.Model.Clone();
            updatedModel.Messages = updatedModel.Messages
                                                .Concat(new[] {new MessageModel{Name = "MessageX"}})
                                                .ToArray();
            OnMessage(new ModelUpdated(updatedModel,set));
        }

        private void OnMessagesCollected(MessageCollection<AddAgentRequested, ModelCreated> set)
        {
            set.MarkAsConsumed(set.Message1);

            CommunityModel updatedModel = set.Message2.Model.Clone();
            updatedModel.Agents = updatedModel.Agents
                                                .Concat(new[] {new AgentModel(){Name = "AgentX"}})
                                                .ToArray();
            OnMessage(new ModelUpdated(updatedModel,set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            addAgentCollector.TryPush(messageData);
            addMessageCollector.TryPush(messageData);
            addGeneratorSettingsCollector.TryPush(messageData);
        }
    }
}
