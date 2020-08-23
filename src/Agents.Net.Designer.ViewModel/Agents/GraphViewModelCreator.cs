﻿using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(GraphViewModelCreated))]
    public class GraphViewModelCreator : Agent
    {
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            OnMessage(new GraphViewModelCreated(new GraphViewModel(), messageData));
        }
    }
}