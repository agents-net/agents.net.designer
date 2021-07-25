#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Linq;
using Agents.Net.Designer.Model;
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
                     .FirstOrDefault(m => m.Name == messageX && m.MessageType == MessageType.Message)
                     .Should().NotBeNull($"the message should be in the view model:{Environment.NewLine}" +
                                         string.Join(Environment.NewLine, viewModel.Flatten()));
        }

        [Then(@"the graph contains the interceptor agent ""(.*)""")]
        public void ThenTheGraphContainsTheInterceptorAgent(string name)
        {
            scenarioContext.WaitForSilentPulse();
            
            GraphViewModel viewModel = scenarioContext.Get<GraphViewModel>(StringConstants.GraphViewModelCreated);
            InterceptorAgentModel model = viewModel.Graph.Nodes.Select(n => n.UserData)
                                                   .OfType<InterceptorAgentModel>()
                                                   .FirstOrDefault(i => i.Name == name);
            model.Should()
                 .NotBeNull(
                     $"the interceptor {name} was expected to exist in the graph. Available nodes are:{Environment.NewLine}{string.Join(Environment.NewLine, viewModel.Graph.Nodes.Select(n => n.UserData))}");
        }

        [Then(@"the tree contains the message decorator ""(.*)""")]
        public void ThenTheTreeContainsTheMessageDecorator(string name)
        {
            scenarioContext.WaitForSilentPulse();
            
            TreeViewModel viewModel = scenarioContext.Get<TreeViewModel>(StringConstants.TreeViewModelCreated);
            viewModel.Flatten().OfType<MessageViewModel>()
                     .FirstOrDefault(m => m.Name == name && m.MessageType == MessageType.MessageDecorator)
                     .Should().NotBeNull($"the message decorator should be in the view model:{Environment.NewLine}" +
                                         string.Join(Environment.NewLine, viewModel.Flatten()));
        }

        [Then(@"the tree contains the message with the full name ""(.*)""")]
        public void ThenTheTreeContainsTheMessageWithTheFullName(string fullName)
        {
            scenarioContext.WaitForSilentPulse();
            
            TreeViewModel viewModel = scenarioContext.Get<TreeViewModel>(StringConstants.TreeViewModelCreated);
            viewModel.Flatten().OfType<MessageViewModel>()
                     .FirstOrDefault(m => m.FullName == fullName && m.MessageType == MessageType.Message)
                     .Should().NotBeNull($"the message should be in the view model:{Environment.NewLine}" +
                                         string.Join(Environment.NewLine, viewModel.Flatten()));
        }
    }
}