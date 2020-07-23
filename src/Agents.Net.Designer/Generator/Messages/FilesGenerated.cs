using System.Collections.Generic;
using System.Linq;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class FilesGenerated : Message
    {        public FilesGenerated(string[] paths, IEnumerable<FileGenerated> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Paths = paths;
            PredecessorMessages = predecessorMessages;
        }

        public string[] Paths { get; }

        //TODO remove with resolved https://github.com/agents-net/agents.net/issues/48
        public IEnumerable<FileGenerated> PredecessorMessages { get; }

        protected override string DataToString()
        {
            return $"{nameof(Paths)}: {Paths.Length}; {nameof(PredecessorMessages)}: {PredecessorMessages.Count()}";
        }
    }
}
