using System.Collections.ObjectModel;

namespace Agents.Net.Designer.ViewModel
{
    public class CommunityViewModel : FolderViewModel
    {
        private bool generateAutofacModule;

        public CommunityViewModel()
        {
            Name = "<Root>";
        }

        public bool GenerateAutofacModule
        {
            get => generateAutofacModule;
            set
            {
                if (value == generateAutofacModule) return;
                generateAutofacModule = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TreeViewItem> BuildInTypes { get; } = new();
    }
}