using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class AddGeneratorSettingsRequested : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition AddGeneratorSettingsRequestedDefinition { get; } =
            new MessageDefinition(nameof(AddGeneratorSettingsRequested));

        #endregion

        public AddGeneratorSettingsRequested(Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, AddGeneratorSettingsRequestedDefinition, childMessages)
        {
        }

        public AddGeneratorSettingsRequested(IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, AddGeneratorSettingsRequestedDefinition, childMessages)
        {
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
