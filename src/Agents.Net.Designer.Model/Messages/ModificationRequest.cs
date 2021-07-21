#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModificationRequest : Message
    {
        public ModificationRequest(Message predecessorMessage, params Modification[] modifications)
            : base(predecessorMessage)
        {
            this.Modifications = modifications;
        }

        public ModificationRequest(IEnumerable<Message> predecessorMessages, params Modification[] modifications)
            : base(predecessorMessages)
        {
            this.Modifications = modifications;
        }

        public IReadOnlyCollection<Modification> Modifications { get; }

        protected override string DataToString()
        {
            return string.Join(Environment.NewLine, Modifications);
        }
    }
}