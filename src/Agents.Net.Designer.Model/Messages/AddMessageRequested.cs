using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddMessageRequested : Message
    {
        public AddMessageRequested(Message predecessorMessage)
            : base(predecessorMessage)
        {
        }

        public AddMessageRequested(IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
