using System.Collections.Generic;

namespace Agents.Net.Designer.Json.Messages
{
    public class JsonModelSourceChanged : Message
    {        public JsonModelSourceChanged(string jsonModel, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            JsonModel = jsonModel;
        }

        public JsonModelSourceChanged(string jsonModel, IEnumerable<Message> predecessorMessages,
                                      params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            JsonModel = jsonModel;
        }

        public string JsonModel { get; }

        protected override string DataToString()
        {
            return $"{nameof(JsonModel)}: {JsonModel.Length}";
        }
    }
}
