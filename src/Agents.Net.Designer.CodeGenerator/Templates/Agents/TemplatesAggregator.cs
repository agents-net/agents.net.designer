using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.CodeGenerator.Templates.Messages;

namespace Agents.Net.Designer.CodeGenerator.Templates.Agents
{
    [Consumes(typeof(TemplateLoaded))]
    [Produces(typeof(TemplatesLoaded))]
    public class TemplatesAggregator : Agent
    {
        private readonly MessageAggregator<TemplateLoaded> aggregator;

        public TemplatesAggregator(IMessageBoard messageBoard) : base(messageBoard)
        {
            aggregator = new MessageAggregator<TemplateLoaded>(OnAggregated);
        }

        private void OnAggregated(IReadOnlyCollection<TemplateLoaded> messages)
        {
            MessageDomain.TerminateDomainsOf(messages);
            OnMessage(new TemplatesLoaded(messages.ToDictionary(m => m.Name, m => m.Content), messages));
        }

        protected override void ExecuteCore(Message messageData)
        {
            aggregator.Aggregate(messageData);
        }
    }
}
