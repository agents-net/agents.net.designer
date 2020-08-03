using System;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    [Consumes(typeof(ModifyModel))]
    [Consumes(typeof(TreeViewModelCreated))]
    [Produces(typeof(ViewModelChangeApplying))]
    public class CommunityViewModelUpdater : Agent
    {
        private readonly MessageCollector<TreeViewModelCreated, ModifyModel> collector;

        public CommunityViewModelUpdater(IMessageBoard messageBoard) : base(messageBoard)
        {
            collector = new MessageCollector<TreeViewModelCreated, ModifyModel>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<TreeViewModelCreated, ModifyModel> set)
        {
            set.MarkAsConsumed(set.Message2);
            if (!(set.Message2.Target is CommunityModel) &&
                !(set.Message2.Target is GeneratorSettings))
            {
                return;
            }

            CommunityViewModel changingViewModel = set.Message1.ViewModel.Community;
            OnMessage(new ViewModelChangeApplying(() =>
            {
                switch (set.Message2.Property)
                {
                    case GeneratorSettingsPackageNamespaceProperty _:
                        string newName = string.IsNullOrEmpty(set.Message2.NewValue.AssertTypeOf<string>())
                                             ? "<Root>"
                                             : set.Message2.NewValue.AssertTypeOf<string>();
                        changingViewModel.Name = newName;
                        RestructureViewModels(set.Message2, changingViewModel);
                        break;
                    case GeneratorSettingsGenerateAutofacProperty _:
                        changingViewModel.GenerateAutofacModule = set.Message2.NewValue.AssertTypeOf<bool>();
                        break;
                    case PackageMessagesProperty _:
                        ChangeMessages(set.Message2, changingViewModel);
                        break;
                    case PackageAgentsProperty _:
                        ChangeAgents(set.Message2, changingViewModel);
                        break;
                }
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

            string oldNamespace = modifyModel.OldValue.AssertTypeOf<string>() ?? string.Empty;
            string newNamespace = modifyModel.NewValue.AssertTypeOf<string>() ?? string.Empty;

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
                if (!relativeFullName.StartsWith('.'))
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
                FolderViewModel rootFolder = new FolderViewModel
                {
                    Name = newNamespace,
                    IsRelativeRoot = true
                };
                changingViewModel.Items.Add(rootFolder);
            }
        }

        private void ChangeAgents(ModifyModel modifyModel, CommunityViewModel changingViewModel)
        {
            switch (modifyModel.ModificationType)
            {
                case ModelModification.Add:
                    AddAgent();
                    break;
                case ModelModification.Remove:
                    RemoveAgent();
                    break;
                case ModelModification.Change:
                    RemoveAgent();
                    AddAgent();
                    break;
            }

            void AddAgent()
            {
                AgentModel agentModel = modifyModel.NewValue.AssertTypeOf<AgentModel>();
                AgentViewModel viewModel = agentModel.CreateViewModel(changingViewModel);
                changingViewModel.AddItem(viewModel);
            }

            void RemoveAgent()
            {
                AgentModel agentModel = modifyModel.OldValue.AssertTypeOf<AgentModel>();
                AgentViewModel viewModel = (AgentViewModel) changingViewModel.FindViewItemById(agentModel.Id);
                changingViewModel.RemoveItem(viewModel);
            }
        }

        private void ChangeMessages(ModifyModel modifyModel, CommunityViewModel changingViewModel)
        {
            switch (modifyModel.ModificationType)
            {
                case ModelModification.Add:
                    AddMessage();
                    break;
                case ModelModification.Remove:
                    RemoveMessage();
                    break;
                case ModelModification.Change:
                    RemoveMessage();
                    AddMessage();
                    break;
            }

            void AddMessage()
            {
                MessageModel messageModel = modifyModel.NewValue.AssertTypeOf<MessageModel>();
                MessageViewModel viewModel = messageModel.CreateViewModel();
                if (!messageModel.BuildIn)
                {
                    changingViewModel.AddItem(viewModel);
                }

                changingViewModel.FindItemByType<AgentViewModel>()?.AvailableItems.AvailableMessages.Add(viewModel);
            }

            void RemoveMessage()
            {
                MessageModel messageModel = modifyModel.OldValue.AssertTypeOf<MessageModel>();
                MessageViewModel viewModel = (MessageViewModel) changingViewModel.FindViewItemById(messageModel.Id);
                changingViewModel.RemoveItem(viewModel);
                changingViewModel.FindItemByType<AgentViewModel>()?.AvailableItems.AvailableMessages
                                 .Remove(viewModel);
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
