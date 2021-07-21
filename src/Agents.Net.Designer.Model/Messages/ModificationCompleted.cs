#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModificationCompleted : Message
    {
        public ModificationCompleted(Message predecessorMessage, CommunityModel model)
            : base(predecessorMessage)
        {
            Model = model;
        }

        public ModificationCompleted(IEnumerable<Message> predecessorMessages, CommunityModel model)
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