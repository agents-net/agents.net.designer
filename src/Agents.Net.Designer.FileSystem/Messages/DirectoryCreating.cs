#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class DirectoryCreating : FileSystemMessage
    {
        public DirectoryCreating(string path, Message predecessorMessage)
			: base(path, predecessorMessage)
        {
        }

        public DirectoryCreating(string path, IEnumerable<Message> predecessorMessages)
			: base(path, predecessorMessages)
        {
        }
    }
}
