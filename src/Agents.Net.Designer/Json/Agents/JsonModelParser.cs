using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;

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
                JsonSerializer serializer = new JsonSerializer();
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
    }
}
