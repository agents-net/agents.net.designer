using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelLoaded : Message
    {
        public ModelLoaded(CommunityModel model, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Model = model;
        }

        public ModelLoaded(CommunityModel model, IEnumerable<Message> predecessorMessages,
                           params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
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