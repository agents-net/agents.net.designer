using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelModified : Message
    {
        public ModelModified(Message predecessorMessage, CommunityModel model)
            : base(predecessorMessage)
        {
            Model = model;
        }

        public ModelModified(IEnumerable<Message> predecessorMessages, CommunityModel model)
            : base(predecessorMessages)
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