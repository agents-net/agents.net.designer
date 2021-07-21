#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(ModificationResult))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Produces(typeof(ViewModelChangeApplying))]
    [Produces(typeof(TreeViewModelUpdated))]
    public class CommunityViewModelUpdater : Agent
    {
        private readonly MessageCollector<TreeViewModelCreated, ModifyModel, ModificationResult> collector;

        public CommunityViewModelUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<TreeViewModelCreated, ModifyModel, ModificationResult>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TreeViewModelCreated, ModifyModel, ModificationResult> set)
        {
            set.MarkAsConsumed(set.Message2);
            set.MarkAsConsumed(set.Message3);
            if (set.Message2.Modification.Target is not CommunityModel &&
                set.Message2.Modification.Target is not GeneratorSettings)
            {
                return;
            }

            CommunityViewModel changingViewModel = set.Message1.ViewModel.Community;
            OnMessage(new ViewModelChangeApplying(() =>
            {
                switch (set.Message2.Modification.Property)
                {
                    case GeneratorSettingsPackageNamespaceProperty _:
                        string newName = string.IsNullOrEmpty(set.Message2.Modification.NewValue.AssertTypeOf<string>())
                                             ? "<Root>"
                                             : set.Message2.Modification.NewValue.AssertTypeOf<string>();
                        changingViewModel.Name = newName;
                        RestructureViewModels(set.Message2, changingViewModel);
                        break;
                    case GeneratorSettingsGenerateAutofacProperty _:
                        changingViewModel.GenerateAutofacModule = set.Message2.Modification.NewValue.AssertTypeOf<bool>();
                        break;
                    case PackageMessagesProperty _:
                        ChangeMessages(set.Message2, changingViewModel);
                        break;
                    case PackageAgentsProperty _:
                        ChangeAgents(set.Message2, changingViewModel);
                        break;
                }
                OnMessage(new TreeViewModelUpdated(set));
            }, set));
        }

        private void RestructureViewModels(ModifyModel modifyModel, CommunityViewModel changingViewModel)
        {
            AgentViewModel[] agents = changingViewModel.FindItemsByType<AgentViewModel>().ToArray();
            MessageViewModel[] messages = changingViewModel.FindItemsByType<MessageViewModel>().ToArray();

            foreach (AgentViewModel agent in agents)
            {
                changingViewModel.RemoveItem(agent);
            }

            foreach (MessageViewModel message in messages)
            {
                changingViewModel.RemoveItem(message);
            }

            string oldNamespace = modifyModel.Modification.OldValue.AssertTypeOf<string>() ?? string.Empty;
            string newNamespace = modifyModel.Modification.NewValue.AssertTypeOf<string>() ?? string.Empty;

            UpdateRelativeRootFolder(changingViewModel, oldNamespace, newNamespace);

            foreach (AgentViewModel agent in agents)
            {
                agent.FullName = UpdateFullName(agent.FullName, agent.RelativeNamespace);
                changingViewModel.AddItem(agent);
            }

            foreach (MessageViewModel message in messages)
            {
                message.FullName = UpdateFullName(message.FullName, message.RelativeNamespace);
                changingViewModel.AddItem(message);
            }

            string UpdateFullName(string oldFullName, string relativeNamespace)
            {
                if (!relativeNamespace.StartsWith("."))
                {
                    return oldFullName;
                }
                string relativeFullName = oldFullName.Substring(oldNamespace.Length);
                if (!relativeFullName.StartsWith(".", StringComparison.Ordinal))
                {
                    relativeFullName = $".{relativeFullName}";
                }

                return string.IsNullOrEmpty(newNamespace)
                           ? relativeFullName.Substring(1)
                           : newNamespace + relativeFullName;
            }
        }

        private static void UpdateRelativeRootFolder(CommunityViewModel changingViewModel, string oldNamespace,
                                                     string newNamespace)
        {
            if (!string.IsNullOrEmpty(oldNamespace))
            {
                FolderViewModel rootFolder = changingViewModel.Items.OfType<FolderViewModel>()
                                                              .First(f => f.IsRelativeRoot);
                if (!string.IsNullOrEmpty(newNamespace))
                {
                    rootFolder.Name = newNamespace;
                }
                else
                {
                    changingViewModel.RemoveItem(rootFolder);
                }
            }
            else if (!string.IsNullOrEmpty(newNamespace))
            {
                FolderViewModel rootFolder = new()
                {
                    Name = newNamespace,
                    IsRelativeRoot = true
                };
                changingViewModel.Items.Add(rootFolder);
            }
        }

        private void ChangeAgents(ModifyModel modifyModel, CommunityViewModel changingViewModel)
        {
            switch (modifyModel.Modification.ModificationType)
            {
                case ModificationType.Add:
                    AddAgent();
                    break;
                case ModificationType.Remove:
                    RemoveAgent();
                    break;
                case ModificationType.Change:
                    RemoveAgent();
                    AddAgent();
                    break;
            }

            void AddAgent()
            {
                AgentModel agentModel = modifyModel.Modification.NewValue.AssertTypeOf<AgentModel>();
                AgentViewModel viewModel = agentModel.CreateViewModel(changingViewModel);
                changingViewModel.AddItem(viewModel);
                
                (changingViewModel.FindItemByType<AgentViewModel>()?.AvailableItems
                 ??changingViewModel.FindItemByType<MessageViewModel>()?.AvailableItems)?.AvailableAgents.Add(viewModel);
            }

            void RemoveAgent()
            {
                AgentModel agentModel = modifyModel.Modification.OldValue.AssertTypeOf<AgentModel>();
                AgentViewModel viewModel = (AgentViewModel) changingViewModel.FindViewItemById(agentModel.Id);
                changingViewModel.RemoveItem(viewModel);
                
                (changingViewModel.FindItemByType<AgentViewModel>()?.AvailableItems
                 ??changingViewModel.FindItemByType<MessageViewModel>()?.AvailableItems)?.AvailableAgents.Remove(viewModel);
            }
        }

        private void ChangeMessages(ModifyModel modifyModel, CommunityViewModel changingViewModel)
        {
            switch (modifyModel.Modification.ModificationType)
            {
                case ModificationType.Add:
                    AddMessage();
                    break;
                case ModificationType.Remove:
                    RemoveMessage();
                    break;
                case ModificationType.Change:
                    RemoveMessage();
                    AddMessage();
                    break;
            }

            void AddMessage()
            {
                MessageModel messageModel = modifyModel.Modification.NewValue.AssertTypeOf<MessageModel>();
                MessageViewModel viewModel = messageModel.CreateViewModel(changingViewModel);
                changingViewModel.AddItem(viewModel);

                (changingViewModel.FindItemByType<AgentViewModel>()?.AvailableItems
                                 ??changingViewModel.FindItemByType<MessageViewModel>()?.AvailableItems)?.AvailableMessages.Add(viewModel);
            }

            void RemoveMessage()
            {
                MessageModel messageModel = modifyModel.Modification.OldValue.AssertTypeOf<MessageModel>();
                MessageViewModel viewModel = (MessageViewModel) changingViewModel.FindViewItemById(messageModel.Id);
                changingViewModel.RemoveItem(viewModel);

                (changingViewModel.FindItemByType<AgentViewModel>()?.AvailableItems
                 ??changingViewModel.FindItemByType<MessageViewModel>()?.AvailableItems)?.AvailableMessages.Remove(viewModel);
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
