using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class FilesGenerated : Message
    {
        public FilesGenerated(string[] paths, IEnumerable<FileGenerated> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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
