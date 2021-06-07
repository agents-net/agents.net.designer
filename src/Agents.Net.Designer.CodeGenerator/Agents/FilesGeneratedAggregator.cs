using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.CodeGenerator.Messages;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Consumes(typeof(FileGenerated))]
    [Produces(typeof(FilesGenerated))]
    public class FilesGeneratedAggregator : Agent
    {
        private readonly MessageAggregator<FileGenerated> aggregator;

        public FilesGeneratedAggregator(IMessageBoard messageBoard) : base(messageBoard)
        {
            aggregator = new MessageAggregator<FileGenerated>(OnAggregated);
        }

        private void OnAggregated(IReadOnlyCollection<FileGenerated> set)
        {
            MessageDomain.TerminateDomainsOf(set);
            OnMessage(new FilesGenerated(set.Select(f => f.Result).ToArray(), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            aggregator.Aggregate(messageData);
        }
    }
}
