using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Templates.Agents
{
    public class TemplatesAggregator : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition TemplatesAggregatorDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      TemplateLoaded.TemplateLoadedDefinition
                                  },
                                  new []
                                  {
                                      TemplatesLoaded.TemplatesLoadedDefinition
                                  });

        #endregion

        private readonly MessageAggregator<TemplateLoaded> aggregator;

        public TemplatesAggregator(IMessageBoard messageBoard) : base(TemplatesAggregatorDefinition, messageBoard)
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
