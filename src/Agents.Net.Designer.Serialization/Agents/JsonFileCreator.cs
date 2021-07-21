#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.IO;
using Agents.Net.Designer.FileSystem.Messages;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Serialization.Agents
{
    [Consumes(typeof(FileConnectionVerified))]
    [Consumes(typeof(FileCreated))]
    [Consumes(typeof(FileSystemException))]
    [Produces(typeof(FileConnected))]
    public class JsonFileCreator : Agent
    {
        private readonly MessageGate<FileCreating, FileCreated> gate = new();
        public JsonFileCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if(!messageData.TryGet(out FileConnectionVerified fileVerified))
            {
                gate.Check(messageData);
                return;
            }
            if (fileVerified.FileExist)
            {
                return;
            }

            gate.SendAndContinue(new FileCreating(fileVerified.FileName, fileVerified), OnMessage,
                                 result =>
                                 {
                                     if (result.Result == MessageGateResultKind.Success)
                                     {
                                         OnMessage(new FileConnected(fileVerified.FileName, true, result.EndMessage));
                                     }
                                 });
        }
    }
}
