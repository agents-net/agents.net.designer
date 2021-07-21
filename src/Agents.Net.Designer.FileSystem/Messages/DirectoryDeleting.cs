#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class DirectoryDeleting : FileSystemMessage
    {
        public DirectoryDeleting(string path, Message predecessorMessage, bool recursive = true)
			: base(path, predecessorMessage)
        {
            Recursive = recursive;
        }

        public DirectoryDeleting(string path, IEnumerable<Message> predecessorMessages, bool recursive = true)
			: base(path, predecessorMessages)
        {
            Recursive = recursive;
        }
        
        public bool Recursive { get; }

        protected override string DataToString()
        {
            return $"{nameof(Recursive)}: {Recursive}; {base.DataToString()}";
        }
    }
}
