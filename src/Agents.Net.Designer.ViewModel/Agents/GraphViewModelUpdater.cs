#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(GraphCreated))]
    [Consumes(typeof(GraphCreationSkipped))]
    [Consumes(typeof(GraphViewModelCreated))]
    [Produces(typeof(GraphViewModelUpdated))]
    public class GraphViewModelUpdater : Agent
    {
        private readonly MessageCollector<GraphCreated, GraphViewModelCreated> collector;
        
        public GraphViewModelUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<GraphCreated, GraphViewModelCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<GraphCreated, GraphViewModelCreated> set)
        {
            set.Message2.ViewModel.LastGraphCreatedMessage = set.Message1; 
            set.Message2.ViewModel.Graph = set.Message1.Graph;
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.Is<GraphCreationSkipped>())
            {
                OnMessage(new GraphViewModelUpdated(messageData));
            }
            else
            {
                collector.Push(messageData);
            }
        }
    }
}
