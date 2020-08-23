using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class GeneratingFile : Message
    {        public GeneratingFile(string name, string ns, string path, Message predecessorMessage,
                              params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Name = name;
            Namespace = ns;
            Path = path;
        }

        public GeneratingFile(string name, string ns, string path, IEnumerable<Message> predecessorMessages,
                              params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Name = name;
            Namespace = ns;
            Path = path;
        }

        public string Name { get; }
        public string Namespace { get; }
        public string Path { get; }

        protected override string DataToString()
        {
            return $"{nameof(Name)}: {Name}; {nameof(Namespace)}: {Namespace}; " +
                   $"{nameof(Path)}: {Path}";
        }
    }
}
