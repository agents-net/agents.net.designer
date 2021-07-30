#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.ComponentModel;
using Agents.Net.Designer.ViewModel.Messages;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(GraphViewModelCreated))]
    [Produces(typeof(GraphScopeChanged))]
    public class GraphViewModelWatcher : Agent
    {
        private GraphViewModelCreated created;
        public GraphViewModelWatcher(IMessageBoard messageBoard)
            : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            created = messageData.Get<GraphViewModelCreated>();
            created.ViewModel.PropertyChanged += ViewModelOnPropertyChanged;
        }

        private void ViewModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(GraphViewModel.Scope))
            {
                OnMessage(new GraphScopeChanged(created, created.ViewModel.Scope));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                created.ViewModel.PropertyChanged -= ViewModelOnPropertyChanged;
            }
            base.Dispose(disposing);
        }
    }
}