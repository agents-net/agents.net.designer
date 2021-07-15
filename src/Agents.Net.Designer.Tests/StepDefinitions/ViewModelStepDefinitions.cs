#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Linq;
using Agents.Net.Designer.Tests.Tools;
using Agents.Net.Designer.ViewModel;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.StepDefinitions
{
    [Binding]
    public class ViewModelStepDefinitions
    {
        private readonly ScenarioContext scenarioContext;

        public ViewModelStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Then(@"the tree contains the message ""(.*)""")]
        public void ThenTheTreeContainsTheMessage(string messageX)
        {
            scenarioContext.WaitForSilentPulse();
            
            TreeViewModel viewModel = scenarioContext.Get<TreeViewModel>(StringConstants.TreeViewModelCreated);
            viewModel.Flatten().OfType<MessageViewModel>()
                     .FirstOrDefault(m => m.Name == messageX)
                     .Should().NotBeNull($"the message should be in the view model:{Environment.NewLine}" +
                                         string.Join(Environment.NewLine, viewModel.Flatten()));
        }
    }
}