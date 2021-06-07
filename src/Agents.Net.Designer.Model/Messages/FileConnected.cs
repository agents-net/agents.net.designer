using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class FileConnected : Message
    {
        public FileConnected(string fileName, bool wasCreated, Message predecessorMessage)
            : base(predecessorMessage)
        {
            FileName = fileName;
            WasCreated = wasCreated;
        }

        public FileConnected(string fileName, bool wasCreated, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            FileName = fileName;
            WasCreated = wasCreated;
        }

        public string FileName { get; }
        public bool WasCreated { get; }

        protected override string DataToString()
        {
            return $"{nameof(FileName)}: {FileName}; {nameof(WasCreated)}: {WasCreated}";
        }
    }
}
