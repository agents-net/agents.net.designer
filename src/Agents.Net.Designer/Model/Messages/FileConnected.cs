using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class FileConnected : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition FileConnectedDefinition { get; } =
            new MessageDefinition(nameof(FileConnected));

        #endregion

        public FileConnected(string fileName, bool wasCreated, Message predecessorMessage,
                             params Message[] childMessages)
            : base(predecessorMessage, FileConnectedDefinition, childMessages)
        {
            FileName = fileName;
            WasCreated = wasCreated;
        }

        public FileConnected(string fileName, bool wasCreated, IEnumerable<Message> predecessorMessages,
                             params Message[] childMessages)
            : base(predecessorMessages, FileConnectedDefinition, childMessages)
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
