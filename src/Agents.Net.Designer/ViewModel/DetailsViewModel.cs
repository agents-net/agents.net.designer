using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Agents.Net.Designer.Annotations;

namespace Agents.Net.Designer.ViewModel
{
    public class DetailsViewModel : INotifyPropertyChanged
    {
        private TreeViewItem currentItem;

        public TreeViewItem CurrentItem
        {
            get => currentItem;
            set
            {
                if (Equals(value, currentItem)) return;
                currentItem = value;
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