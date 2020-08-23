using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Agents.Net.Designer.Serialization.Messages
{
    public class JsonModelValidated : Message
    {        public JsonModelValidated(JObject validatedModel, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            ValidatedModel = validatedModel;
        }

        public JsonModelValidated(JObject validatedModel, IEnumerable<Message> predecessorMessages,
                                  params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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
