using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddMessageRequested : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition AddMessageRequestedDefinition { get; } =
            new MessageDefinition(nameof(AddMessageRequested));

        #endregion

        public AddMessageRequested(Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, AddMessageRequestedDefinition, childMessages)
        {
        }

        public AddMessageRequested(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, AddMessageRequestedDefinition, childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
