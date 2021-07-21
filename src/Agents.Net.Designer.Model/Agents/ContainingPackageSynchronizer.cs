#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModificationResult))]
    [Intercepts(typeof(ModelLoaded))]
    public class ContainingPackageSynchronizer : InterceptorAgent
    {
        public ContainingPackageSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            CommunityModel model = messageData.TryGet(out ModificationResult result)
                                       ? result.Model
                                       : messageData.Get<ModelLoaded>().Model;
            foreach (AgentModel agent in model.Agents)
            {
                agent.ContainingPackage = model;
            }

            foreach (MessageModel message in model.Messages)
            {
                message.ContainingPackage = model;
            }

            return InterceptionAction.Continue;
        }
    }
}
