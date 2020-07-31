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
            if (!(set.Message2.Target is AgentModel oldModel))
            {
                return;
            }

            AgentViewModel changingViewModel = (AgentViewModel) set.Message1.ViewModel.Community.FindViewItemById(oldModel.Id);
            OnMessage(new ViewModelChangeApplying(() =>
            {
                switch (set.Message2.Property)
                {
                    case AgentNameProperty _:
                        changingViewModel.Name = set.Message2.NewValue.AssertTypeOf<string>();
                        changingViewModel.FullName = $"{changingViewModel.Namespace}.{changingViewModel.Name}";
                        break;
                    case AgentNamespaceProperty _:
                        changingViewModel.Namespace = set.Message2.NewValue.AssertTypeOf<string>();
                        changingViewModel.FullName = $"{changingViewModel.Namespace}.{changingViewModel.Name}";
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
                    case AgentIncomingEventsProperty _:
                        ChangeEvents(changingViewModel.IncomingEvents, set.Message2);
                        break;
                    case AgentProducedEventsProperty _:
                        ChangeEvents(changingViewModel.ProducedEvents, set.Message2);
                        break;
                }
            }, set));
        }

        private void ChangeEvents(ObservableCollection<string> events, ModifyModel modifyModel)
        {
            switch (modifyModel.ModificationType)
            {
                case ModelModification.Add:
                    events.Add(modifyModel.NewValue.AssertTypeOf<string>());
                    break;
                case ModelModification.Remove:
                    events.Remove(modifyModel.OldValue.AssertTypeOf<string>());
                    break;
                case ModelModification.Change:
                    events.Remove(modifyModel.OldValue.AssertTypeOf<string>());
                    events.Add(modifyModel.NewValue.AssertTypeOf<string>());
                    break;
            }
        }

        private void ChangeMessages(ObservableCollection<MessageViewModel> messages, ModifyModel modifyModel, ObservableCollection<MessageViewModel> availableMessages)
        {
            switch (modifyModel.ModificationType)
            {
                case ModelModification.Add:
                {
                    MessageViewModel viewModel = GetViewModel(modifyModel.NewValue, true);
                    messages.Add(viewModel);
                    break;
                }
                case ModelModification.Remove:
                {
                    MessageViewModel viewModel = GetViewModel(modifyModel.OldValue, false);
                    messages.Remove(viewModel);
                    break;
                }
                case ModelModification.Change:
                {
                    MessageViewModel addViewModel = GetViewModel(modifyModel.NewValue, true);
                    messages.Add(addViewModel);
                    MessageViewModel removeViewModel = GetViewModel(modifyModel.OldValue, false);
                    messages.Remove(removeViewModel);
                    break;
                }
            }

            MessageViewModel GetViewModel(object value, bool generateMock)
            {
                if (value is Guid id)
                {
                    return availableMessages.First(m => m.ModelId == id);
                }

                string name = (string) value;
                MessageViewModel viewModel = availableMessages.FirstOrDefault(m => m.FullName.EndsWith(name))
                                             ?? (generateMock
                                                     ? name.GenerateMessageMock(availableMessages)
                                                     : null);
                return viewModel;
            }
        }

        private void RestructureViewModel(AgentViewModel changingViewModel, TreeViewModel viewModel)
        {
            viewModel.Community.RemoveItem(changingViewModel);
            viewModel.Community.AddItem(changingViewModel);
            changingViewModel.IsSelected = true;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}