using System.Collections.Generic;
using Agents.Net;
using Newtonsoft.Json.Linq;

namespace Agents.Net.Designer.Json.Messages
{
    public class JsonModelValidated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition JsonModelValidatedDefinition { get; } =
            new MessageDefinition(nameof(JsonModelValidated));

        #endregion

        public JsonModelValidated(JObject validatedModel, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, JsonModelValidatedDefinition, childMessages)
        {
            ValidatedModel = validatedModel;
        }

        public JsonModelValidated(JObject validatedModel, IEnumerable<Message> predecessorMessages,
                                  params Message[] childMessages)
            : base(predecessorMessages, JsonModelValidatedDefinition, childMessages)
        {
            ValidatedModel = validatedModel;
        }

        public JObject ValidatedModel { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
