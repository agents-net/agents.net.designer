#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileConnectionVerified : Message
    {
        public FileConnectionVerified(string fileName, bool fileExist, Message predecessorMessage)
            : base(predecessorMessage)
        {
            FileName = fileName;
            FileExist = fileExist;
        }

        public FileConnectionVerified(string fileName, bool fileExist, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            FileName = fileName;
            FileExist = fileExist;
        }

        public string FileName { get; }

        public bool FileExist { get; }

        protected override string DataToString()
        {
            return $"{nameof(FileName)}: {FileName}; {nameof(FileExist)}: {FileExist}";
        }
    }
}
