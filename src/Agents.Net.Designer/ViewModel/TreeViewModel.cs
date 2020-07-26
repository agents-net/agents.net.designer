﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Agents.Net.Designer.Annotations;

namespace Agents.Net.Designer.ViewModel
{
    public class TreeViewModel : INotifyPropertyChanged
    {
        public TreeViewModel()
        {
            Community = new CommunityViewModel();
        }

        public IEnumerable<TreeViewItem> Items
        {
            get => items;
            set
            {
                if (Equals(value, items)) return;
                items = value;
                OnPropertyChanged();
            }
        }

        private CommunityViewModel community;
        private IEnumerable<TreeViewItem> items;

        public CommunityViewModel Community
        {
            get => community;
            set
            {
                if (Equals(value, community)) return;
                community = value;
                OnPropertyChanged();
                Items = new[] {community};
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}