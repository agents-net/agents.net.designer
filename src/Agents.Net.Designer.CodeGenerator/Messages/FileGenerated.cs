﻿using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class FileGenerated : Message
    {        public FileGenerated(string path, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Path = path;
        }

        public FileGenerated(string path, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
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
