#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Serialization.Messages;

namespace Agents.Net.Designer.Serialization.Agents
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

        private readonly object syncRoot = new();
        private JsonTextUpdated updatingMessage;
        private bool fileSynchronizationEnabled;

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.Is<FileConnected>())
            {
                fileSynchronizationEnabled = true;
            }
            else if(messageData.MessageDomain.Root.TryGet(out FileSynchronizationProcessing processing))
            {
                MessageDomain.TerminateDomainsOf(processing);
                lock (syncRoot)
                {
                    if (updatingMessage != processing.Get<JsonTextUpdated>() &&
                        updatingMessage != null)
                    {
                        MessageDomain.CreateNewDomainsFor(FileSynchronizationProcessing.Decorate(updatingMessage));
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
                MessageDomain.CreateNewDomainsFor(FileSynchronizationProcessing.Decorate(updatingMessage));
                return InterceptionAction.Continue;
            }
        }
    }
}
