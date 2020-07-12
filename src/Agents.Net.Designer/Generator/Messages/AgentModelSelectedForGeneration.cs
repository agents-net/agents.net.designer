using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Messages
{
    public class AgentModelSelectedForGeneration : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition AgentModelSelectedForGenerationDefinition { get; } =
            new MessageDefinition(nameof(AgentModelSelectedForGeneration));

        #endregion

        public AgentModelSelectedForGeneration(AgentModel agent, MessageModel[] consumingMessages,
                                               MessageModel[] producingMessages, Message predecessorMessage,
                                               params Message[] childMessages)
            : base(predecessorMessage, AgentModelSelectedForGenerationDefinition, childMessages)
        {
            Agent = agent;
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
        }

        public AgentModelSelectedForGeneration(AgentModel agent, MessageModel[] consumingMessages,
                                               MessageModel[] producingMessages,
                                               IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, AgentModelSelectedForGenerationDefinition, childMessages)
        {
            Agent = agent;
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
        }

        public AgentModel Agent { get; }

        public MessageModel[] ConsumingMessages { get; }

        public MessageModel[] ProducingMessages { get; }

        protected override string DataToString()
        {
            return $"{nameof(Agent)}: {Agent}";
        }
    }
}
