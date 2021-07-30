#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Agents.Net.Designer.ViewModel.MicrosoftGraph.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private Graph graph;
        private GraphViewScope scope;

        public Graph Graph
        {
            get => graph;
            set
            {
                if (Equals(value, graph)) return;
                graph = value;
                OnPropertyChanged();
            }
        }

        public GraphViewScope Scope
        {
            get => scope;
            set
            {
                if (Equals(value, scope)) return;
                scope = value;
                OnPropertyChanged();
            }
        }

        public GraphCreated LastGraphCreatedMessage { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
