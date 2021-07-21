#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileSystemMessage : Message
    {
        public FileSystemMessage(string path, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Path = path;
        }

        public FileSystemMessage(string path, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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