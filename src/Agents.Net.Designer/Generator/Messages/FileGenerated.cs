using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class FileGenerated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition FileGeneratedDefinition { get; } =
            new MessageDefinition(nameof(FileGenerated));

        #endregion

        public FileGenerated(string path, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, FileGeneratedDefinition, childMessages)
        {
            Path = path;
        }

        public FileGenerated(string path, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, FileGeneratedDefinition, childMessages)
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
