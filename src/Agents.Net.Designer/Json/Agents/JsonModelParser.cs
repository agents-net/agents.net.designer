using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Newtonsoft.Json.Serialization;

namespace Agents.Net.Designer.Json.Agents
{
    [Consumes(typeof(JsonTextLoaded))]
    [Produces(typeof(ModelUpdated))]
    [Produces(typeof(ModelLoaded))]
    [Produces(typeof(JsonModelParsingError))]
    public class JsonModelParser : Agent
    {
        public JsonModelParser(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            JsonTextLoaded loaded = messageData.Get<JsonTextLoaded>();

            try
            {
                JsonSerializer serializer = new JsonSerializer
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    SerializationBinder = new AgentsNetSerializationBinder()
                };
                using StringReader reader = new StringReader(loaded.Text);
                using JsonTextReader jsonReader = new JsonTextReader(reader);
                CommunityModel model = serializer.Deserialize<CommunityModel>(jsonReader);
                
                OnMessage(new ModelLoaded(model, messageData, new ModelUpdated(model, messageData)));
            }
            catch (JsonReaderException e)
            {
                OnMessage(new JsonModelParsingError(new []{e.Message}, messageData));
                return;
            }
        }

        private class AgentsNetSerializationBinder : DefaultSerializationBinder
        {
            public override Type BindToType(string? assemblyName, string typeName)
            {
                Type result = base.BindToType(assemblyName, typeName);
                if (result.Assembly.FullName != Assembly.GetAssembly(typeof(AgentModel)).FullName)
                {
                    throw new JsonReaderException($"Cannot deserialize {assemblyName}:{typeName} as it is not type from this program.");
                }
                return result;
            }
        }
    }
}
