using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Agents.Net;

namespace Agents.Net.Designer.FileSystem.Messages
{
    public class FileSystemException : ExceptionMessage
    {
        public FileSystemException(ExceptionDispatchInfo exceptionInfo, Message message, Agent agent)
            : base(exceptionInfo, message, agent)
        {
        }

        public FileSystemException(ExceptionDispatchInfo exceptionInfo, IEnumerable<Message> messages, Agent agent)
            : base(exceptionInfo, messages, agent)
        {
        }
    }
}
