using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel
{
    public class GraphViewModel : INotifyPropertyChanged
    {
        private Graph graph;

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

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
