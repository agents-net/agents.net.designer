#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class ConnectFileRequested : Message
    {
        public ConnectFileRequested(string fileName, Message predecessorMessage)
            : base(predecessorMessage)
        {
            FileName = fileName;
        }

        public ConnectFileRequested(string fileName, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            FileName = fileName;
        }

        public string FileName { get; }

        protected override string DataToString()
        {
            return $"{nameof(FileName)}: {FileName}";
        }
    }
}
