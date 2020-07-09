using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using Agents.Net.Designer.Annotations;

namespace Agents.Net.Designer.ViewModel
{
    public class JsonViewModel : INotifyPropertyChanged
    {
        public JsonViewModel()
        {
            text = "Hello";
        }

        private string text;

        public string Text
        {
            get => text;
            set
            {
                if (value == text) return;
                text = value;
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
