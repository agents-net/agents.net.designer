using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddAgentRequested : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition AddAgentRequestedDefinition { get; } =
            new MessageDefinition(nameof(AddAgentRequested));

        #endregion

        public AddAgentRequested(Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, AddAgentRequestedDefinition, childMessages)
        {
        }

        public AddAgentRequested(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, AddAgentRequestedDefinition, childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
