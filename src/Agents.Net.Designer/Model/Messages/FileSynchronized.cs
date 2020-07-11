using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class FileSynchronized : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition FileSynchronizedDefinition { get; } =
            new MessageDefinition(nameof(FileSynchronized));

        #endregion

        public FileSynchronized(string fileName, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, FileSynchronizedDefinition, childMessages)
        {
            FileName = fileName;
        }

        public FileSynchronized(string fileName, IEnumerable<Message> predecessorMessages,
                                params Message[] childMessages)
            : base(predecessorMessages, FileSynchronizedDefinition, childMessages)
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
