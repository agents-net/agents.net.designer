using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Messages
{
    public class MessageModelSelectedForGeneration : Message
    {        public MessageModelSelectedForGeneration(MessageModel message, Message predecessorMessage,
                                                 params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Message = message;
        }

        public MessageModelSelectedForGeneration(MessageModel message, IEnumerable<Message> predecessorMessages,
                                                 params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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
