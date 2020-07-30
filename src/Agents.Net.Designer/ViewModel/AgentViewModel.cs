using System;
using System.Collections.ObjectModel;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.ViewModel
{
    public class AgentViewModel : TreeViewItem
    {
        private string fullName;
        private string ns;
        private ObservableCollection<string> incomingEvents;
        private ObservableCollection<string> producedEvents;
        private ObservableCollection<MessageViewModel> consumingMessages;
        private ObservableCollection<MessageViewModel> producingMessages;
        private ObservableCollection<MessageViewModel> availableMessages;
        private string newConsumingMessage;
        private string newProducingMessage;
        private object newConsumingMessageObject;
        private object newProducingMessageObject;

        internal Guid ModelId { get; set; }

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

        public string NewConsumingMessage
        {
            get => newConsumingMessage;
            set
            {
                if (value == newConsumingMessage) return;
                newConsumingMessage = value;
                OnPropertyChanged();
            }
        }

        public object NewConsumingMessageObject
        {
            get => newConsumingMessageObject;
            set
            {
                if (Equals(value, newConsumingMessageObject)) return;
                newConsumingMessageObject = value;
                OnPropertyChanged();
            }
        }

        public string NewProducingMessage
        {
            get => newProducingMessage;
            set
            {
                if (value == newProducingMessage) return;
                newProducingMessage = value;
                OnPropertyChanged();
            }
        }

        public object NewProducingMessageObject
        {
            get => newProducingMessageObject;
            set
            {
                if (Equals(value, newProducingMessageObject)) return;
                newProducingMessageObject = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> IncomingEvents
        {
            get => incomingEvents;
            set
            {
                if (Equals(value, incomingEvents)) return;
                incomingEvents = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<string> ProducedEvents
        {
            get => producedEvents;
            set
            {
                if (Equals(value, producedEvents)) return;
                producedEvents = value;
                OnPropertyChanged();
            }
        }

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

        public ObservableCollection<MessageViewModel> ConsumingMessages
        {
            get => consumingMessages;
            set
            {
                if (Equals(value, consumingMessages)) return;
                consumingMessages = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<MessageViewModel> ProducingMessages
        {
            get => producingMessages;
            set
            {
                if (Equals(value, producingMessages)) return;
                producingMessages = value;
                OnPropertyChanged();
            }
        }
    }
}