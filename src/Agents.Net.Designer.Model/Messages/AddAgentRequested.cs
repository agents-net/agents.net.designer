using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddAgentRequested : Message
    {
        public AddAgentRequested(Message predecessorMessage)
            : base(predecessorMessage)
        {
        }

        public AddAgentRequested(IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
