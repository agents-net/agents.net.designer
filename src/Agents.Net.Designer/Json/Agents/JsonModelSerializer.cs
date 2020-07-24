using System;
using System.IO;
using System.Text;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model.Messages;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Json.Agents
{
    [Consumes(typeof(ModelCreated))]
    [Produces(typeof(JsonTextUpdated))]
    public class JsonModelSerializer : Agent
    {
        public JsonModelSerializer(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            ModelCreated created = messageData.Get<ModelCreated>();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
            StringBuilder updatedText = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(updatedText);
            serializer.Serialize(stringWriter, created.Model);
            OnMessage(new JsonTextUpdated(updatedText.ToString(), messageData));
        }
    }
}
