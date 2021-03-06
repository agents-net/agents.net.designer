#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class DirectoryRenamed : FileSystemMessage
    {
        public DirectoryRenamed(string path, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
        }

        public DirectoryRenamed(string path, IEnumerable<Message> predecessorMessages)
			: base(path, predecessorMessages)
        {
        }
    }
}
