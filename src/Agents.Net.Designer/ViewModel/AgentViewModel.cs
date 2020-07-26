namespace Agents.Net.Designer.ViewModel
{
    public class AgentViewModel : TreeViewItem
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