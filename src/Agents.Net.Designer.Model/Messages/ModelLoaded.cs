using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelLoaded : Message
    {
        public ModelLoaded(CommunityModel model, Message predecessor)
            : base(predecessor)
        {
            Model = model;
        }
        
        public ModelLoaded(CommunityModel model, IEnumerable<Message> predecessors)
            : base(predecessors)
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