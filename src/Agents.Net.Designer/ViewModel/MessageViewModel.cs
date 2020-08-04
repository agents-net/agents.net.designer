using System;

namespace Agents.Net.Designer.ViewModel
{
    public class MessageViewModel : TreeViewItem
    {
        private string fullName;
        private string relativeNamespace;

        internal Guid ModelId { get; set; }

        internal bool BuildIn { get; set; }

        public string FullName
        {
            get => fullName;
            set
            {
                if (value == fullName) return;
                fullName = value;
                OnPropertyChanged();
            }
        }

        public string RelativeNamespace
        {
            get => relativeNamespace;
            set
            {
                if (value == relativeNamespace) return;
                relativeNamespace = value;
                OnPropertyChanged();
            }
        }
    }
}