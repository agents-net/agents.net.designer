using System;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModelUpdated))]
    public class ContainingPackageSynchronizer : InterceptorAgent
    {
        public ContainingPackageSynchronizer(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            ModelUpdated updated = messageData.Get<ModelUpdated>();
            foreach (AgentModel agent in updated.Model.Agents)
            {
                agent.ContainingPackage = updated.Model;
            }

            foreach (MessageModel message in updated.Model.Messages)
            {
                message.ContainingPackage = updated.Model;
            }

            return InterceptionAction.Continue;
        }
    }
}
