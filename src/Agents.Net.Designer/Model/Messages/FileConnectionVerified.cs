using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class FileConnectionVerified : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition FileConnectionVerifiedDefinition { get; } =
            new MessageDefinition(nameof(FileConnectionVerified));

        #endregion

        public FileConnectionVerified(string fileName, bool fileExist, Message predecessorMessage,
                                      params Message[] childMessages)
            : base(predecessorMessage, FileConnectionVerifiedDefinition, childMessages)
        {
            FileName = fileName;
            FileExist = fileExist;
        }

        public FileConnectionVerified(string fileName, bool fileExist, IEnumerable<Message> predecessorMessages,
                                      params Message[] childMessages)
            : base(predecessorMessages, FileConnectionVerifiedDefinition, childMessages)
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
