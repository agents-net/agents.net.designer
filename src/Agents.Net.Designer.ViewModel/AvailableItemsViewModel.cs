using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Agents.Net.Designer.ViewModel
{
    public class AvailableItemsViewModel
    {
        public AvailableItemsViewModel(IEnumerable<MessageViewModel> messages, IEnumerable<AgentViewModel> agents)
        {
            AvailableMessages = new ObservableCollection<MessageViewModel>(messages);
            AvailableAgents = new ObservableCollection<AgentViewModel>(agents);
        }

        public AvailableItemsViewModel()
        {
            AvailableMessages = new ObservableCollection<MessageViewModel>();
            AvailableAgents = new ObservableCollection<AgentViewModel>();
        }

        public ObservableCollection<MessageViewModel> AvailableMessages { get; }
        
        public ObservableCollection<AgentViewModel> AvailableAgents { get; }
    }
}
