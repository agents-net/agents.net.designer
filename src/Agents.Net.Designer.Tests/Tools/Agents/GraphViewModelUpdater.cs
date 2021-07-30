#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.ComponentModel;
using Agents.Net.Designer.ViewModel.Messages;
using Microsoft.Msagl.Core.Geometry;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Drawing;

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
                foreach (Node node in created.ViewModel.Graph.Nodes)
                {
                    node.GeometryNode = new Microsoft.Msagl.Core.Layout.Node(new RoundedRect(new Rectangle(0,0,1,1), 0,0));
                }
                OnMessage(new GraphViewModelUpdated(created.ViewModel.LastGraphCreatedMessage));
            }
        }
    }
}