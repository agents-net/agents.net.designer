using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class GeneratingAgent : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition GeneratingAgentDefinition { get; } =
            new MessageDefinition(nameof(GeneratingAgent));

        #endregion

        public GeneratingAgent(string[] consumingMessages,
                               string[] producingMessages, string[] dependencies, Message predecessorMessage,
                               params Message[] childMessages)
            : base(predecessorMessage, GeneratingAgentDefinition, childMessages)
        {
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
            Dependencies = dependencies;
        }

        public GeneratingAgent(string[] consumingMessages,
                               string[] producingMessages, string[] dependencies,
                               IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, GeneratingAgentDefinition, childMessages)
        {
            ConsumingMessages = consumingMessages;
            ProducingMessages = producingMessages;
            Dependencies = dependencies;
        }
        public string[] ConsumingMessages { get; }
        public string[] ProducingMessages { get; }
        public string[] Dependencies { get; }

        protected override string DataToString()
        {
            return $"{nameof(ConsumingMessages)}: {string.Join(", ",ConsumingMessages)}; " +
                   $"{nameof(ProducingMessages)}: {string.Join(", ",ProducingMessages)}; " +
                   $"{nameof(Dependencies)}: {string.Join(", ",Dependencies)}; ";
        }
    }
}
