using System;
using System.IO;
using System.Runtime.ExceptionServices;
using Agents.Net.Designer.FileSystem.Messages;

namespace Agents.Net.Designer.FileSystem.Agents
{
    [Produces(typeof(FileSystemException))]
    public abstract class FileSystemAgent : Agent
    {
        protected FileSystemAgent(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected sealed override void ExecuteCore(Message messageData)
        {
            try
            {
                SafeExecuteCore(messageData);
            }
            catch (PathTooLongException e)
            {
                OnMessage(new FileSystemException(ExceptionDispatchInfo.Capture(e), messageData, this));
            }
            catch (DirectoryNotFoundException e)
            {
                OnMessage(new FileSystemException(ExceptionDispatchInfo.Capture(e), messageData, this));
            }
            catch (UnauthorizedAccessException e)
            {
                OnMessage(new FileSystemException(ExceptionDispatchInfo.Capture(e), messageData, this));
            }
            catch (IOException e)
            {
                OnMessage(new FileSystemException(ExceptionDispatchInfo.Capture(e), messageData, this));
            }
        }

        protected abstract void SafeExecuteCore(Message messageData);
    }
}