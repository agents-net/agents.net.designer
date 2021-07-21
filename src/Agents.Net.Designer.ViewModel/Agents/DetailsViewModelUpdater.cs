#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(DetailsViewModelCreated))]
    [Consumes(typeof(SelectedTreeViewItemChanged))]
    public class DetailsViewModelUpdater : Agent
    {
        private readonly MessageCollector<DetailsViewModelCreated, SelectedTreeViewItemChanged> collector;
        public DetailsViewModelUpdater(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<DetailsViewModelCreated, SelectedTreeViewItemChanged>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<DetailsViewModelCreated, SelectedTreeViewItemChanged> set)
        {
            set.Message1.ViewModel.CurrentItem = set.Message2.SelectedItem;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}