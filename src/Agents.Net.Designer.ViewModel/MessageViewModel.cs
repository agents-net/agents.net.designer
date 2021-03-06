#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;

namespace Agents.Net.Designer.ViewModel
{
    public class MessageViewModel : TreeViewItem
    {
        private string fullName;
        private string relativeNamespace;
        private AvailableItemsViewModel availableItems;
        private MessageViewModel decoratedMessage;
        private MessageType messageType;

        internal Guid ModelId { get; set; }

        internal bool BuildIn { get; set; }

        internal bool IsGeneric { get; set; }
        
        internal int GenericParameterCount { get; set; }
        
        internal bool IsGenericInstance { get; set; }

        public MessageType MessageType
        {
            get => messageType;
            set
            {
                if (Equals(value, messageType)) return;
                messageType = value;
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

        public MessageViewModel DecoratedMessage
        {
            get => decoratedMessage;
            set
            {
                if (value == decoratedMessage) return;
                decoratedMessage = value;
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
    }
}