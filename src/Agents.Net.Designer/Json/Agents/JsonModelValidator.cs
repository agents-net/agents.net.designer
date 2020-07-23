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
    [Consumes(typeof(JsonModelSourceChanged))]
    [Produces(typeof(JsonModelParsingError))]
    [Produces(typeof(JsonModelValidated))]
    public class JsonModelValidator : Agent
    {        private readonly JSchema schema;
        public JsonModelValidator(IMessageBoard messageBoard) : base(messageBoard)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            schema = generator.Generate(typeof(CommunityModel));
        }

        protected override void ExecuteCore(Message messageData)
        {
            JsonModelSourceChanged sourceChanged = messageData.Get<JsonModelSourceChanged>();

            try
            {
                JObject validationObject = JObject.Parse(sourceChanged.JsonModel, new JsonLoadSettings{LineInfoHandling = LineInfoHandling.Load});
                validationObject.IsValid(schema, out IList<string> messages);
                
                if (messages.Any())
                {
                    OnMessage(new JsonModelParsingError(messages, messageData));
                }
                else
                {
                    OnMessage(new JsonModelValidated(validationObject, messageData));
                }
            }
            catch (JsonReaderException e)
            {
                OnMessage(new JsonModelParsingError(new []{e.Message}, messageData));
            }
        }
    }
}
