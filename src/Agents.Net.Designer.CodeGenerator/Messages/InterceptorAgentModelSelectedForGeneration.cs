using System.Collections.Generic;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class InterceptorAgentModelSelectedForGeneration : Message
    {
        public InterceptorAgentModelSelectedForGeneration(MessageModel[] interceptingMessages, Message predecessorMessage,
                                                          params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            InterceptingMessages = interceptingMessages;
        }

        public InterceptorAgentModelSelectedForGeneration(MessageModel[] interceptingMessages,
                                                          IEnumerable<Message> predecessorMessages,
                                                          params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            InterceptingMessages = interceptingMessages;
        }

        public MessageModel[] InterceptingMessages { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
