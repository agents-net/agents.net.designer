#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages
{
    public class GraphCreated : Message
    {
        public GraphCreated(Graph graph, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Graph = graph;
        }

        public GraphCreated(Graph graph, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
