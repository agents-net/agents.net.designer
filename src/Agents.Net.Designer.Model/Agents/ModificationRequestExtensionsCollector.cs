#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModificationRequestExtending))]
    [Produces(typeof(ModificationRequestExtended))]
    public class ModificationRequestExtensionsCollector : Agent
    {
        public ModificationRequestExtensionsCollector(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            ModificationRequestExtending extending = messageData.Get<ModificationRequestExtending>();
            bool extended = true;
            List<Modification> modifications = new(extending.Modifications);
            while (extended)
            {
                extended = extending.Extender.Aggregate(false, (current, extender) => current | extender(modifications));
            }
            OnMessage(new ModificationRequestExtended(messageData, modifications));
        }
    }
}