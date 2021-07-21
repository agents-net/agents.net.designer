#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(DetailsViewModelCreated))]
    public class DetailsViewModelCreator : Agent
    {
        public DetailsViewModelCreator(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            OnMessage(new DetailsViewModelCreated(new DetailsViewModel(), messageData));
        }
    }
}