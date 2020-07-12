using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ExportImageRequested : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition ExportImageRequestedDefinition { get; } =
            new MessageDefinition(nameof(ExportImageRequested));

        #endregion

        public ExportImageRequested(string path, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, ExportImageRequestedDefinition, childMessages)
        {
            Path = path;
        }

        public ExportImageRequested(string path, IEnumerable<Message> predecessorMessages,
                                    params Message[] childMessages)
            : base(predecessorMessages, ExportImageRequestedDefinition, childMessages)
        {
            Path = path;
        }

        public string Path { get; }

        protected override string DataToString()
        {
            return $"{nameof(Path)}: {Path}";
        }
    }
}
