using System;
using System.IO;
using System.Text;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Json.Agents
{
    public class JsonFileLoader : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition JsonFileLoaderDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      FileConnectionVerified.FileConnectionVerifiedDefinition
                                  },
                                  new []
                                  {
                                      FileConnected.FileConnectedDefinition,
                                      JsonTextUpdated.JsonTextUpdatedDefinition
                                  });

        #endregion

        public JsonFileLoader(IMessageBoard messageBoard) : base(JsonFileLoaderDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            FileConnectionVerified fileVerified = messageData.Get<FileConnectionVerified>();
            if (!fileVerified.FileExist)
            {
                return;
            }

            OnMessage(new JsonTextUpdated(File.ReadAllText(fileVerified.FileName, Encoding.UTF8), messageData));
            OnMessage(new FileConnected(fileVerified.FileName, false, messageData));
        }
    }
}
