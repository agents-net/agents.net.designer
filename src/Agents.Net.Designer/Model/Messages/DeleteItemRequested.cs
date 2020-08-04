using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class DeleteItemRequested : Message
    {
        public DeleteItemRequested(Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
        }

        public DeleteItemRequested(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
