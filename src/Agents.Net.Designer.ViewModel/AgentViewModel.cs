using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.ViewModel
{
    public class AgentViewModel : TreeViewItem
    {
        private string fullName;
        private ObservableCollection<string> incomingEvents;
        private ObservableCollection<string> producedEvents;
        private ObservableCollection<MessageViewModel> consumingMessages;
        private ObservableCollection<MessageViewModel> producingMessages;
        private ObservableCollection<MessageViewModel> interceptingMessages;
        private string newConsumingMessage;
        private string newProducingMessage;
        private object newConsumingMessageObject;
        private object newProducingMessageObject;
        private AvailableItemsViewModel availableItems;
        private string newIncomingEvent;
        private string newProducedEvent;
        private string relativeNamespace;
        private AgentType agentType;
        private string newInterceptingMessage;
        private object newInterceptingMessageObject;

        public AgentViewModel()
        {
            DeleteItemCommand = new RelayCommand(DeleteItem);
        }

        private void DeleteItem(object obj)
        {
            OnDeleteItemRequested((DeleteItemEventArgs) obj);
        }

        internal Guid ModelId { get; set; }

        internal event EventHandler<DeleteItemEventArgs> DeleteItemRequested; 

        public ICommand DeleteItemCommand { get; }

        public AgentType AgentType
        {
            get => agentType;
            set
            {
                if (value == agentType) return;
                agentType = value;
                OnPropertyChanged();
            }
        }

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

        public string NewInterceptingMessage
        {
            get => newInterceptingMessage;
            set
            {
                if (value == newInterceptingMessage) return;
                newInterceptingMessage = value;
                OnPropertyChanged();
            }
        }

        public object NewInterceptingMessageObject
        {
            get => newInterceptingMessageObject;
            set
            {
                if (Equals(value, newInterceptingMessageObject)) return;
                newInterceptingMessageObject = value;
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

        public string NewIncomingEvent
        {
            get => newIncomingEvent;
            set
            {
                if (value == newIncomingEvent) return;
                newIncomingEvent = value;
                OnPropertyChanged();
            }
        }

        public string NewProducedEvent
        {
            get => newProducedEvent;
            set
            {
                if (value == newProducedEvent) return;
                newProducedEvent = value;
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

        public ObservableCollection<MessageViewModel> InterceptingMessages
        {
            get => interceptingMessages;
            set
            {
                if (Equals(value, interceptingMessages)) return;
                interceptingMessages = value;
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

        public AvailableItemsViewModel AvailableItems
        {
            get => availableItems;
            set
            {
                if (Equals(value, availableItems)) return;
                availableItems = value;
                OnPropertyChanged();
            }
        }

        protected virtual void OnDeleteItemRequested(DeleteItemEventArgs e)
        {
            DeleteItemRequested?.Invoke(this, e);
        }
    }
}