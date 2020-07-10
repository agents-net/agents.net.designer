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
    public class JsonModelParser : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition JsonModelParserDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      JsonModelValidated.JsonModelValidatedDefinition
                                  },
                                  new []
                                  {
                                      ModelCreated.ModelCreatedDefinition
                                  });

        #endregion

        private readonly JSchema schema;
        public JsonModelParser(IMessageBoard messageBoard) : base(JsonModelParserDefinition, messageBoard)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            schema = generator.Generate(typeof(CommunityModel));
        }

        protected override void ExecuteCore(Message messageData)
        {
            JsonModelValidated validated = messageData.Get<JsonModelValidated>();

            try
            {
                JsonSerializer serializer = new JsonSerializer();
                CommunityModel model = serializer.Deserialize<CommunityModel>(validated.ValidatedModel.CreateReader());
                
                OnMessage(new ModelCreated(model, messageData));
            }
            catch (JsonReaderException e)
            {
                OnMessage(new JsonModelParsingError(new []{e.Message}, messageData));
                return;
            }
        }
    }
}
