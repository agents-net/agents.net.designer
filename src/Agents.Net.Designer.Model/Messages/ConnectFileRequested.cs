﻿using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ConnectFileRequested : Message
    {
            : base(predecessorMessage, childMessages:childMessages)
        {
            FileName = fileName;
        }

        public ConnectFileRequested(string fileName, IEnumerable<Message> predecessorMessages,
                                    params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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