﻿using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelUpdated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition ModelUpdatedDefinition { get; } =
            new MessageDefinition(nameof(ModelUpdated));

        #endregion

        public ModelUpdated(CommunityModel model, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, ModelUpdatedDefinition, childMessages)
        {
            Model = model;
        }

        public ModelUpdated(CommunityModel model, IEnumerable<Message> predecessorMessages,
                            params Message[] childMessages)
            : base(predecessorMessages, ModelUpdatedDefinition, childMessages)
        {
            Model = model;
        }

        public CommunityModel Model { get; }

        protected override string DataToString()
        {
            return $"{nameof(Model)}: {Model}";
        }
    }
}