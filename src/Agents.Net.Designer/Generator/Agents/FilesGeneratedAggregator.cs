using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    [Consumes(typeof(FileGenerated))]
    [Produces(typeof(FilesGenerated))]
    public class FilesGeneratedAggregator : Agent
    {        private readonly MessageAggregator<FileGenerated> aggregator;

        public FilesGeneratedAggregator(IMessageBoard messageBoard) : base(messageBoard)
        {
            aggregator = new MessageAggregator<FileGenerated>(OnAggregated);
        }

        private void OnAggregated(ICollection<FileGenerated> set)
        {
            MessageDomain.TerminateDomainsOf(set.ToArray());
            OnMessage(new FilesGenerated(set.Select(f => f.Path).ToArray(), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            aggregator.Aggregate(messageData);
        }
    }
}
