using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.MicrosoftGraph.Messages
{
    public class GraphCreated : Message
    {        public GraphCreated(Graph graph, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Graph = graph;
        }

        public GraphCreated(Graph graph, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Graph = graph;
        }

        public Graph Graph { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
