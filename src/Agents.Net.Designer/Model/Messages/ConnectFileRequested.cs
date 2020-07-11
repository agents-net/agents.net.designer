using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ConnectFileRequested : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition ConnectFileRequestedDefinition { get; } =
            new MessageDefinition(nameof(ConnectFileRequested));

        #endregion

        public ConnectFileRequested(string fileName, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, ConnectFileRequestedDefinition, childMessages)
        {
            FileName = fileName;
        }

        public ConnectFileRequested(string fileName, IEnumerable<Message> predecessorMessages,
                                    params Message[] childMessages)
            : base(predecessorMessages, ConnectFileRequestedDefinition, childMessages)
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
