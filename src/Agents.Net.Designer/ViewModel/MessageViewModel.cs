namespace Agents.Net.Designer.ViewModel
{
    public class MessageViewModel : TreeViewItem
    {
        private string fullName;

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
    }
}