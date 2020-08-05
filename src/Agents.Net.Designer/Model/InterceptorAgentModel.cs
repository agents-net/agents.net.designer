using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class InterceptorAgentModel : AgentModel
    {
        public InterceptorAgentModel(string name = "", string @namespace = ".Agents", Guid[] consumingMessages = null,
                                     Guid[] producedMessages = null, string[] incomingEvents = null, string[] producedEvents = null,
                                     Guid id = default, Guid[] interceptingMessages = null)
            : base(name, @namespace, consumingMessages,
                   producedMessages, incomingEvents, producedEvents,
                   id)
        {
            InterceptingMessages = interceptingMessages ?? new Guid[0];
        }

        public override AgentModel Clone(string name = null, string @namespace = null, Guid[] consumingMessages = null,
                                         Guid[] producedMessages = null, string[] incomingEvents = null, string[] producedEvents = null,
                                         Guid id = default)
        {
            return new InterceptorAgentModel(name ?? Name,
                                             @namespace ?? Namespace,
                                             consumingMessages ?? ConsumingMessages,
                                             producedMessages ?? ProducedMessages,
                                             incomingEvents ?? IncomingEvents,
                                             producedEvents ?? ProducedEvents,
                                             id == default ? Id : id,
                                             InterceptingMessages);
        }

        public virtual AgentModel Clone(Guid[] interceptingMessages, string name = null, string @namespace = null, Guid[] consumingMessages = null,
                                         Guid[] producedMessages = null, string[] incomingEvents = null, string[] producedEvents = null,
                                         Guid id = default)
        {
            return new InterceptorAgentModel(name ?? Name,
                                             @namespace ?? Namespace,
                                             consumingMessages ?? ConsumingMessages,
                                             producedMessages ?? ProducedMessages,
                                             incomingEvents ?? IncomingEvents,
                                             producedEvents ?? ProducedEvents,
                                             id == default ? Id : id,
                                             interceptingMessages);
        }

        public Guid[] InterceptingMessages { get; }
    }
}
