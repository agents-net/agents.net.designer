using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Templates.Agents
{
    [Consumes(typeof(TemplateLoaded))]
    [Produces(typeof(TemplatesLoaded))]
    public class TemplatesAggregator : Agent
    {        private readonly MessageAggregator<TemplateLoaded> aggregator;

        public TemplatesAggregator(IMessageBoard messageBoard) : base(messageBoard)
        {
            aggregator = new MessageAggregator<TemplateLoaded>(OnAggregated);
        }

        private void OnAggregated(ICollection<TemplateLoaded> messages)
        {
            MessageDomain.TerminateDomainsOf(messages.ToArray());
            OnMessage(new TemplatesLoaded(messages.ToDictionary(m => m.Name, m => m.Content), messages));
        }

        protected override void ExecuteCore(Message messageData)
        {
            aggregator.Aggregate(messageData);
        }
    }
}
