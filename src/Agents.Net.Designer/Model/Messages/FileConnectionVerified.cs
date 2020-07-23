using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class FileConnectionVerified : Message
    {        public FileConnectionVerified(string fileName, bool fileExist, Message predecessorMessage,
                                      params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            FileName = fileName;
            FileExist = fileExist;
        }

        public FileConnectionVerified(string fileName, bool fileExist, IEnumerable<Message> predecessorMessages,
                                      params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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
