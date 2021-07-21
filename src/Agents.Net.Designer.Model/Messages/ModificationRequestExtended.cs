#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModificationRequestExtended : Message
    {
        public ModificationRequestExtended(Message predecessorMessage, IReadOnlyCollection<Modification> modifications)
            : base(predecessorMessage)
        {
            Modifications = modifications;
        }

        public ModificationRequestExtended(IEnumerable<Message> predecessorMessages, IReadOnlyCollection<Modification> modifications)
            : base(predecessorMessages)
        {
            Modifications = modifications;
        }

        public IReadOnlyCollection<Modification> Modifications { get; }

        protected override string DataToString()
        {
            return string.Join(Environment.NewLine, Modifications);
        }
    }
}