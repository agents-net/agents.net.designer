using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
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
