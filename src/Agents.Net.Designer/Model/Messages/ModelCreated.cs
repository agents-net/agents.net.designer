using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelCreated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition ModelCreatedDefinition { get; } =
            new MessageDefinition(nameof(ModelCreated));

        #endregion

        public ModelCreated(CommunityModel model, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, ModelCreatedDefinition, childMessages)
        {
            Model = model;
        }

        public ModelCreated(CommunityModel model, IEnumerable<Message> predecessorMessages,
                            params Message[] childMessages)
            : base(predecessorMessages, ModelCreatedDefinition, childMessages)
        {
            Model = model;
        }

        public CommunityModel Model { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
