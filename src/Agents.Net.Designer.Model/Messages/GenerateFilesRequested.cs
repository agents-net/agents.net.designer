using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class GenerateFilesRequested : Message
    {        public GenerateFilesRequested(string path, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Path = path;
        }

        public GenerateFilesRequested(string path, IEnumerable<Message> predecessorMessages,
                                      params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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
