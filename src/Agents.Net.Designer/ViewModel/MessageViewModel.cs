namespace Agents.Net.Designer.ViewModel
{
    public class MessageViewModel : TreeViewItem
    {
        private string fullName;
        private string ns;

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

        public string Namespace
        {
            get => ns;
            set
            {
                if (value == ns) return;
                ns = value;
                OnPropertyChanged();
            }
        }
    }
}