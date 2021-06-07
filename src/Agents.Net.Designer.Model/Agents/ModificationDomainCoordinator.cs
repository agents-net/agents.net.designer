using System;
using System.Linq;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModifyModel))]
    [Intercepts(typeof(ModelModified))]
    public class ModificationDomainCoordinator : InterceptorAgent
    {
        public ModificationDomainCoordinator(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            if (messageData.Is<ModifyModel>())
            {
                if (messageData.MessageDomain.Root.Is<ModifyModel>())
                {
                    if (messageData.Get<ModifyModel>() == messageData.MessageDomain.Root.Get<ModifyModel>())
                    {
                        //Already its own domain
                        return InterceptionAction.Continue;
                    }
                    MessageDomain.TerminateDomainsOf(messageData);
                }
                MessageDomain.CreateNewDomainsFor(messageData);
            }
            else
            {
                MessageDomain.TerminateDomainsOf(messageData);
            }
            return InterceptionAction.Continue;
        }
    }
}