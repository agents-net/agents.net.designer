﻿using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages
{
    public class GraphCreated : Message
    {
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