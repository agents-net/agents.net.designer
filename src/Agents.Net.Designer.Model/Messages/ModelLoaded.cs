#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

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