using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class GenerateFilesRequested : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition GenerateFilesRequestedDefinition { get; } =
            new MessageDefinition(nameof(GenerateFilesRequested));

        #endregion

        public GenerateFilesRequested(string path, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, GenerateFilesRequestedDefinition, childMessages)
        {
            Path = path;
        }

        public GenerateFilesRequested(string path, IEnumerable<Message> predecessorMessages,
                                      params Message[] childMessages)
            : base(predecessorMessages, GenerateFilesRequestedDefinition, childMessages)
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
