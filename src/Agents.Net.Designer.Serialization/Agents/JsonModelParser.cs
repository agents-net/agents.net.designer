#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.IO;
using System.Reflection;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Serialization.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Agents.Net.Designer.Serialization.Agents
{
    [Consumes(typeof(JsonTextLoaded))]
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
                JsonSerializer serializer = new()
                {
                    TypeNameHandling = TypeNameHandling.Auto,
                    SerializationBinder = new AgentsNetSerializationBinder()
                };
                using StringReader reader = new(loaded.Text);
                using JsonTextReader jsonReader = new(reader);
                CommunityModel model = serializer.Deserialize<CommunityModel>(jsonReader);
                
                OnMessage(new ModelLoaded(model, messageData));
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
