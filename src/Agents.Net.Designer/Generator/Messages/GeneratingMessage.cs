using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class GeneratingMessage : Message
    {        public GeneratingMessage(Message predecessorMessage,
                                 params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
        }

        public GeneratingMessage(IEnumerable<Message> predecessorMessages,
                                 params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
