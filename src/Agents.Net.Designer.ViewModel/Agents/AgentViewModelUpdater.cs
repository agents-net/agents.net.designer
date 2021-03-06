#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Agents;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Produces(typeof(ViewModelChangeApplying))]
    [Produces(typeof(TreeViewModelUpdated))]
    public class AgentViewModelUpdater : Agent
    {
        private readonly MessageCollector<TreeViewModelCreated, ModifyModel> collector;
        public AgentViewModelUpdater(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<TreeViewModelCreated, ModifyModel>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TreeViewModelCreated, ModifyModel> set)
        {
            set.MarkAsConsumed(set.Message2);
            if (set.Message2.Modification.Target is not AgentModel oldModel)
            {
                return;
            }

            AgentViewModel changingViewModel = (AgentViewModel) set.Message1.ViewModel.Community.FindViewItemById(oldModel.Id);
            OnMessage(new ViewModelChangeApplying(() =>
            {
                switch (set.Message2.Modification.Property)
                {
                    case AgentNameProperty _:
                        changingViewModel.Name = set.Message2.Modification.NewValue.AssertTypeOf<string>();
                        string fullNamespace = changingViewModel.FullName.Substring(0,changingViewModel.FullName.Length-set.Message2.Modification.OldValue.AssertTypeOf<string>().Length+1);
                        changingViewModel.FullName =  $"{fullNamespace}.{changingViewModel.Name}";
                        break;
                    case AgentNamespaceProperty _:
                        changingViewModel.RelativeNamespace = set.Message2.Modification.NewValue.AssertTypeOf<string>();
                        changingViewModel.FullName = $"{set.Message2.Modification.NewValue.AssertTypeOf<string>().ExtendNamespace(oldModel)}.{changingViewModel.Name}";
                        RestructureViewModel(changingViewModel, set.Message1.ViewModel);
                        break;
                    case AgentConsumingMessagesProperty _:
                        ChangeMessages(changingViewModel.ConsumingMessages, set.Message2,
                                       changingViewModel.AvailableItems.AvailableMessages);
                        break;
                    case AgentProducedMessagesProperty _:
                        ChangeMessages(changingViewModel.ProducingMessages, set.Message2,
                                       changingViewModel.AvailableItems.AvailableMessages);
                        break;
                    case InterceptorAgentInterceptingMessagesProperty _:
                        ChangeMessages(changingViewModel.InterceptingMessages, set.Message2,
                                       changingViewModel.AvailableItems.AvailableMessages);
                        break;
                    case AgentIncomingEventsProperty _:
                        ChangeEvents(changingViewModel.IncomingEvents, set.Message2);
                        break;
                    case AgentProducedEventsProperty _:
                        ChangeEvents(changingViewModel.ProducedEvents, set.Message2);
                        break;
                }
                OnMessage(new TreeViewModelUpdated(set));
            }, set));
        }

        private void ChangeEvents(ObservableCollection<string> events, ModifyModel modifyModel)
        {
            switch (modifyModel.Modification.ModificationType)
            {
                case ModificationType.Add:
                    events.Add(modifyModel.Modification.NewValue.AssertTypeOf<string>());
                    break;
                case ModificationType.Remove:
                    events.Remove(modifyModel.Modification.OldValue.AssertTypeOf<string>());
                    break;
                case ModificationType.Change:
                    events.Remove(modifyModel.Modification.OldValue.AssertTypeOf<string>());
                    events.Add(modifyModel.Modification.NewValue.AssertTypeOf<string>());
                    break;
            }
        }

        private void ChangeMessages(ObservableCollection<MessageViewModel> messages, ModifyModel modifyModel, ObservableCollection<MessageViewModel> availableMessages)
        {
            switch (modifyModel.Modification.ModificationType)
            {
                case ModificationType.Add:
                {
                    MessageViewModel viewModel = GetViewModel(modifyModel.Modification.NewValue);
                    messages.Add(viewModel);
                    break;
                }
                case ModificationType.Remove:
                {
                    MessageViewModel viewModel = GetViewModel(modifyModel.Modification.OldValue);
                    messages.Remove(viewModel);
                    break;
                }
                case ModificationType.Change:
                {
                    MessageViewModel addViewModel = GetViewModel(modifyModel.Modification.NewValue);
                    messages.Add(addViewModel);
                    MessageViewModel removeViewModel = GetViewModel(modifyModel.Modification.OldValue);
                    messages.Remove(removeViewModel);
                    break;
                }
            }

            MessageViewModel GetViewModel(object value)
            {
                return availableMessages.First(m => m.ModelId == value.AssertTypeOf<Guid>());
            }
        }

        private void RestructureViewModel(AgentViewModel changingViewModel, TreeViewModel viewModel)
        {
            viewModel.Community.RemoveItem(changingViewModel);
            viewModel.Community.AddItem(changingViewModel);
            changingViewModel.Select();
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}