#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.ComponentModel;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.Tests.Tools.Agents
{
    [Consumes(typeof(GraphViewModelCreated))]
    [Produces(typeof(GraphViewModelUpdated))]
    public class GraphViewModelUpdater : Agent
    {
        private GraphViewModelCreated created;

        public GraphViewModelUpdater(IMessageBoard messageBoard, string name = null)
            : base(messageBoard, name)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            created = messageData.Get<GraphViewModelCreated>();
            created.ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(created.ViewModel.Graph))
            {
                OnMessage(new GraphViewModelUpdated(created.ViewModel.LastGraphCreatedMessage));
            }
        }
    }
}