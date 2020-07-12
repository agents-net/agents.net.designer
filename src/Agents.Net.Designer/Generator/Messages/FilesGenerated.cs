using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class FilesGenerated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition FilesGeneratedDefinition { get; } =
            new MessageDefinition(nameof(FilesGenerated));

        #endregion

        public FilesGenerated(string[] paths, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, FilesGeneratedDefinition, childMessages)
        {
            Paths = paths;
        }

        public FilesGenerated(string[] paths, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, FilesGeneratedDefinition, childMessages)
        {
            Paths = paths;
        }

        public string[] Paths { get; }

        protected override string DataToString()
        {
            return $"{nameof(Paths)}: {Paths.Length}";
        }
    }
}
