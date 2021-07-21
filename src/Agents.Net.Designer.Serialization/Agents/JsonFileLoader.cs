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
    [Consumes(typeof(FileConnectionVerified))]
    [Consumes(typeof(FileOpened))]
    [Consumes(typeof(FileSystemException))]
    [Produces(typeof(FileConnected))]
    [Produces(typeof(JsonTextLoaded))]
    public class JsonFileLoader : Agent
    {
        private readonly MessageGate<FileOpening, FileOpened> gate = new();
        public JsonFileLoader(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            if(!messageData.TryGet(out FileConnectionVerified fileVerified))
            {
                gate.Check(messageData);
                return;
            }
            if (!fileVerified.FileExist)
            {
                return;
            }
            
            gate.SendAndContinue(new FileOpening(fileVerified.FileName, messageData), OnMessage, 
                                 result =>
                                 {
                                     if (result.Result == MessageGateResultKind.Success)
                                     {
                                         using StreamReader reader = new(result.EndMessage.Data, Encoding.UTF8);
                                         OnMessage(new JsonTextLoaded(reader.ReadToEnd(), result.EndMessage));
                                         OnMessage(new FileConnected(fileVerified.FileName, false, result.EndMessage));
                                     }
                                 });
        }
    }
}
