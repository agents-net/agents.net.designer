using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddMessageDecoratorRequested : Message
    {
        public AddMessageDecoratorRequested(Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
        }

        public AddMessageDecoratorRequested(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
