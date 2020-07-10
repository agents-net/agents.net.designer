using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.MicrosoftGraph.Messages
{
    public class GraphCreated : Message
    {
        #region Definition

        [MessageDefinition]
        public static MessageDefinition GraphCreatedDefinition { get; } =
            new MessageDefinition(nameof(GraphCreated));

        #endregion

        public GraphCreated(Graph graph, Message predecessorMessage, params Message[] childMessages)
            : base(predecessorMessage, GraphCreatedDefinition, childMessages)
        {
            Graph = graph;
        }

        public GraphCreated(Graph graph, IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, GraphCreatedDefinition, childMessages)
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
