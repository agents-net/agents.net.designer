using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Agents.Net.Designer.ViewModel.Annotations;

namespace Agents.Net.Designer.ViewModel
{
    public class AvailableItemsViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<MessageViewModel> availableMessages;

        public ObservableCollection<MessageViewModel> AvailableMessages
        {
            get => availableMessages;
            set
            {
                if (Equals(value, availableMessages)) return;
                availableMessages = value;
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
