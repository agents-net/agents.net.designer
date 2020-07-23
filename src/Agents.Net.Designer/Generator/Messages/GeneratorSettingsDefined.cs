using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Messages
{
    public class GeneratorSettingsDefined : Message
    {        public GeneratorSettingsDefined(GeneratorSettings settings, string path, Message predecessorMessage,
                                        params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Settings = settings;
            Path = path;
        }

        public GeneratorSettingsDefined(GeneratorSettings settings, string path,
                                        IEnumerable<Message> predecessorMessages,
                                        params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Settings = settings;
            Path = path;
        }

        public GeneratorSettings Settings { get; }
        public string Path { get; }

        protected override string DataToString()
        {
            return $"{nameof(Settings)}: {Settings}, {nameof(Path)}: {Path}";
        }
    }
}
