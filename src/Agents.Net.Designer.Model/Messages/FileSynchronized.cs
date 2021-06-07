using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class FileSynchronized : Message
    {
        public FileSynchronized(string fileName, Message predecessorMessage)
            : base(predecessorMessage)
        {
            FileName = fileName;
        }

        public FileSynchronized(string fileName, IEnumerable<Message> predecessorMessages)
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
