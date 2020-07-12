using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Templates.Messages
{
    public class TemplatesLoaded : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition TemplatesLoadedDefinition { get; } =
            new MessageDefinition(nameof(TemplatesLoaded));

        #endregion

        public TemplatesLoaded(Dictionary<string, string> templates, Message predecessorMessage,
                               params Message[] childMessages)
            : base(predecessorMessage, TemplatesLoadedDefinition, childMessages)
        {
            Templates = templates;
        }

        public TemplatesLoaded(Dictionary<string, string> templates, IEnumerable<Message> predecessorMessages,
                               params Message[] childMessages)
            : base(predecessorMessages, TemplatesLoadedDefinition, childMessages)
        {
            Templates = templates;
        }

        public Dictionary<string, string> Templates { get; }

        protected override string DataToString()
        {
            return $"{nameof(Templates)}: {string.Join(", ",Templates.Keys)}";
        }
    }
}
