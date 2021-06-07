using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class GeneratingFile : Message
    {
        public GeneratingFile(string name, string ns, string path, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Name = name;
            Namespace = ns;
            Path = path;
        }

        public GeneratingFile(string name, string ns, string path, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
