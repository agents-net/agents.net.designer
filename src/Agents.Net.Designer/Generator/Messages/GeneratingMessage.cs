using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class GeneratingMessage : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition GeneratingMessageDefinition { get; } =
            new MessageDefinition(nameof(GeneratingMessage));

        #endregion

        public GeneratingMessage(Message predecessorMessage,
                                 params Message[] childMessages)
            : base(predecessorMessage, GeneratingMessageDefinition, childMessages)
        {
        }

        public GeneratingMessage(IEnumerable<Message> predecessorMessages,
                                 params Message[] childMessages)
            : base(predecessorMessages, GeneratingMessageDefinition, childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
