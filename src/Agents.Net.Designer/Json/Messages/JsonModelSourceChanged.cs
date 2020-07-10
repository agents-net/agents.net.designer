using System.Collections.Generic;

namespace Agents.Net.Designer.Json.Messages
{
    public class JsonModelSourceChanged : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition JsonModelSourceChangedDefinition { get; } =
            new MessageDefinition(nameof(JsonModelSourceChanged));

        #endregion

        public JsonModelSourceChanged(string jsonModel, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, JsonModelSourceChangedDefinition, childMessages)
        {
            JsonModel = jsonModel;
        }

        public JsonModelSourceChanged(string jsonModel, IEnumerable<Message> predecessorMessages,
                                      params Message[] childMessages)
            : base(predecessorMessages, JsonModelSourceChangedDefinition, childMessages)
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
