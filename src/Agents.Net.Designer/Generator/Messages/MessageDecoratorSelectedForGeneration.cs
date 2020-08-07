using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Messages
{
    public class MessageDecoratorSelectedForGeneration : Message
    {
        public MessageDecoratorSelectedForGeneration(MessageModel decoratedMessage, Message predecessorMessage,
                                                     params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            DecoratedMessage = decoratedMessage;
        }

        public MessageDecoratorSelectedForGeneration(MessageModel decoratedMessage,
                                                     IEnumerable<Message> predecessorMessages,
                                                     params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            DecoratedMessage = decoratedMessage;
        }

        public MessageModel DecoratedMessage { get; }

        protected override string DataToString()
        {
            return $"{nameof(DecoratedMessage)}: {DecoratedMessage}";
        }
    }
}
