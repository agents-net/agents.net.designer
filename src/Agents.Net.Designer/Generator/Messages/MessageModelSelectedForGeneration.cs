using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Messages
{
    public class MessageModelSelectedForGeneration : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition MessageModelSelectedForGenerationDefinition { get; } =
            new MessageDefinition(nameof(MessageModelSelectedForGeneration));

        #endregion

        public MessageModelSelectedForGeneration(MessageModel message, Message predecessorMessage,
                                                 params Message[] childMessages)
            : base(predecessorMessage, MessageModelSelectedForGenerationDefinition, childMessages)
        {
            Message = message;
        }

        public MessageModelSelectedForGeneration(MessageModel message, IEnumerable<Message> predecessorMessages,
                                                 params Message[] childMessages)
            : base(predecessorMessages, MessageModelSelectedForGenerationDefinition, childMessages)
        {
            Message = message;
        }

        public MessageModel Message { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
