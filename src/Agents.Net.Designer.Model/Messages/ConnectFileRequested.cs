using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
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
