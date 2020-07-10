using System;
using System.IO;
using System.Text;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model.Messages;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Json.Agents
{
    public class JsonModelSerializer : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition JsonModelSerializerDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      ModelUpdated.ModelUpdatedDefinition
                                  },
                                  new []
                                  {
                                      JsonTextUpdated.JsonTextUpdatedDefinition
                                  });

        #endregion

        public JsonModelSerializer(IMessageBoard messageBoard) : base(JsonModelSerializerDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            ModelUpdated updated = messageData.Get<ModelUpdated>();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
            StringBuilder updatedText = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(updatedText);
            serializer.Serialize(stringWriter, updated.Model);
            OnMessage(new JsonTextUpdated(updatedText.ToString(), messageData));
        }
    }
}
