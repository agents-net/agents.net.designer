using System.IO;
using System.Text;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Serialization.Messages;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Serialization.Agents
{
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(JsonTextUpdated))]
    public class JsonModelSerializer : Agent
    {
        public JsonModelSerializer(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            ModelUpdated updated = messageData.Get<ModelUpdated>();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new DesignerModelContractResolver()
            });
            StringBuilder updatedText = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(updatedText);
            serializer.Serialize(stringWriter, updated.Model);
            OnMessage(new JsonTextUpdated(updatedText.ToString(), messageData));
        }
    }
}
