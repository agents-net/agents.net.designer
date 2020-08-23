using System;
using System.Collections.Concurrent;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Json.Agents
{
    [Intercepts(typeof(JsonTextUpdated))]
    [Consumes(typeof(FileConnected))]
    [Consumes(typeof(FileSynchronized))]
    [Produces(typeof(JsonTextUpdated))]
    public class FileSynchronizationCoordinator : InterceptorAgent
    {
        public FileSynchronizationCoordinator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        private readonly object syncRoot = new object();
        private JsonTextUpdated updatingMessage;
        private bool fileSynchronizationEnabled;

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.Is<FileConnected>())
            {
                fileSynchronizationEnabled = true;
            }
            else if(messageData.TryGetPredecessor(out JsonTextUpdated processedMessage))
            {
                lock (syncRoot)
                {
                    if (updatingMessage != processedMessage &&
                        updatingMessage != null)
                    {
                        OnMessage(updatingMessage);
                    }
                    else
                    {
                        updatingMessage = null;
                    }
                }
            }
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            if (!fileSynchronizationEnabled)
            {
                return InterceptionAction.Continue;
            }
            JsonTextUpdated textUpdated = messageData.Get<JsonTextUpdated>();
            lock (syncRoot)
            {
                if (updatingMessage != null &&
                    updatingMessage != textUpdated)
                {
                    updatingMessage = textUpdated;
                    return InterceptionAction.DoNotPublish;
                }
                
                updatingMessage = textUpdated;
                return InterceptionAction.Continue;
            }
        }
    }
}
