using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class GeneratingInterceptorAgent : Message
    {
        public GeneratingInterceptorAgent(string[] interceptingMessages, Message predecessorMessage,
                                          params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            InterceptingMessages = interceptingMessages;
        }

        public GeneratingInterceptorAgent(string[] interceptingMessages, IEnumerable<Message> predecessorMessages,
                                          params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            InterceptingMessages = interceptingMessages;
        }

        public string[] InterceptingMessages { get; }

        protected override string DataToString()
        {
            return $"{nameof(InterceptingMessages)}: {string.Join(", ",InterceptingMessages)}; ";
        }
    }
}
