using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class DeleteItemRequested : Message
    {
        public DeleteItemRequested(Message predecessorMessage)
            : base(predecessorMessage)
        {
        }

        public DeleteItemRequested(IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
