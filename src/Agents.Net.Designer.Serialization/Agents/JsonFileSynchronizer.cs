#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.IO;
using System.Text;
using Agents.Net.Designer.FileSystem.Messages;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Serialization.Messages;

namespace Agents.Net.Designer.Serialization.Agents
{
    [Consumes(typeof(FileConnected))]
    [Consumes(typeof(JsonTextUpdated))]
    [Consumes(typeof(FileOpened))]
    [Consumes(typeof(FileSystemException))]
    [Produces(typeof(FileSynchronized))]
    public class JsonFileSynchronizer : Agent
    {
        private MessageCollector<FileConnected, JsonTextUpdated> collector;
        private readonly MessageGate<FileOpening, FileOpened> gate = new();

        public JsonFileSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<FileConnected, JsonTextUpdated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<FileConnected, JsonTextUpdated> set)
        {
            gate.SendAndContinue(new FileOpening(set.Message1.FileName, set), OnMessage, 
                                 result =>
            {
                if (result.Result == MessageGateResultKind.Success)
                {
                    result.EndMessage.Data.SetLength(0);
                    using StreamWriter writer = new(result.EndMessage.Data, Encoding.UTF8);
                    writer.Write(set.Message2.Text);
                    OnMessage(new FileSynchronized(set.Message1.FileName, result.EndMessage));
                }
            });
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out FileConnected connected) &&
                !connected.WasCreated)
            {
                collector = new MessageCollector<FileConnected, JsonTextUpdated>(OnMessagesCollected);
            }

            gate.Check(messageData);
            collector.TryPush(messageData);
        }
    }
}
