#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Linq;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Tests.Tools;
using Agents.Net.Designer.ViewModel;
using Agents.Net.Designer.ViewModel.Messages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.StepDefinitions
{
    public partial class TreeStepDefinitions
    {
        [When(@"I select the agent ""(.*)""")]
        public void WhenISelectTheAgent(string agentName)
        {
            scenarioContext.WaitForSilentPulse();
            TreeViewModel viewModel = scenarioContext.Get<TreeViewModel>(StringConstants.TreeViewModelCreated);

            AgentViewModel viewItem = viewModel.Flatten().OfType<AgentViewModel>()
                                               .FirstOrDefault(
                                                   m => m.Name == agentName && m.AgentType == AgentType.Agent);
            viewItem.Should().NotBeNull($"the agent {agentName} should be visible.");
            viewItem.Select();
            scenarioContext.Get<IMessageBoard>().Publish(new SelectedTreeViewItemChanged(viewItem,scenarioContext.Get<InitializeMessage>()));
        }
    }
}