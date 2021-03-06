#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.IO;
using System.Text;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Serialization.Messages;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Serialization.Agents
{
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(JsonTextUpdated))]
    public class JsonModelSerializer : Agent
    {
        public JsonModelSerializer(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            ModelVersionCreated updated = messageData.Get<ModelVersionCreated>();
            JsonSerializer serializer = JsonSerializer.Create(new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                TypeNameHandling = TypeNameHandling.Auto,
                ContractResolver = new DesignerModelContractResolver()
            });
            StringBuilder updatedText = new();
            using StringWriter stringWriter = new(updatedText);
            serializer.Serialize(stringWriter, updated.Model);
            OnMessage(new JsonTextUpdated(updatedText.ToString(), messageData));
        }
    }
}
