#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Metrolib.Controls;

namespace Agents.Net.Designer.ViewModel
{
    public abstract class TreeViewItem : ITreeViewItemViewModel
    {
        private string name;
        private bool isSelected;
        private bool isExpanded;

        protected TreeViewItem()
        {
            Items.CollectionChanged += ItemsOnCollectionChanged;
        }

        private void ItemsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Add &&
                e.Action != NotifyCollectionChangedAction.Replace)
            {
                return;
            }

            foreach (TreeViewItem added in e.NewItems.Cast<TreeViewItem>())
            {
                added.Parent = this;
            }
        }

        public TreeViewItem Parent { get; private set; }

        public string Name
        {
            get => name;
            set
            {
                if (value == name) return;
                name = value;
                OnPropertyChanged();
            }
        }

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (value == isSelected) return;
                isSelected = value;
                OnPropertyChanged();
            }
        }

        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (value == isExpanded) return;
                isExpanded = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<TreeViewItem> Items { get; } = new();

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public override string ToString()
        {
            return $"{nameof(Parent)}: {Parent?.Name??string.Empty}, {nameof(Name)}: {Name}, {nameof(Items)}: {Items.Count}";
        }
    }
}