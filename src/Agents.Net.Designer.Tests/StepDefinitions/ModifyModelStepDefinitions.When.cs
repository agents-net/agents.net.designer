#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Tests.Tools;
using Agents.Net.Designer.ViewModel;
using Agents.Net.Designer.ViewModel.Messages;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.StepDefinitions
{
    [Binding]
    public sealed class ModifyModelStepDefinitions
    {
        private readonly ScenarioContext scenarioContext;

        public ModifyModelStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }
        
        [When(@"I add an agent to the model")]
        public void WhenIAddAnAgentToTheModel()
        {
            scenarioContext.Get<IMessageBoard>().Publish(new AddAgentRequested(scenarioContext.Get<InitializeMessage>()));
        }

        [When(@"I add a message to the model")]
        public void WhenIAddAMessageToTheModel()
        {
            scenarioContext.Get<IMessageBoard>().Publish(new AddMessageRequested(scenarioContext.Get<InitializeMessage>()));
        }

        [When(@"I add an interceptor agent to the model")]
        public void WhenIAddAnInterceptorAgentToTheModel()
        {
            scenarioContext.Get<IMessageBoard>().Publish(new AddInterceptorAgentRequested(scenarioContext.Get<InitializeMessage>()));
        }

        [When(@"I add a message decorator to the model")]
        public void WhenIAddAMessageDecoratorToTheModel()
        {
            scenarioContext.Get<IMessageBoard>().Publish(new AddMessageDecoratorRequested(scenarioContext.Get<InitializeMessage>()));
        }

        [When(@"I add the (consumed|produced|intercepted) message ""(.*)"" to the agent ""(.*)""")]
        public void WhenIAddTheConsumedMessageToTheAgent(string type, string name, string agentName)
        {
            scenarioContext.WaitForSilentPulse();
            
            TreeViewModel viewModel = scenarioContext.Get<TreeViewModel>(StringConstants.TreeViewModelCreated);
            AgentViewModel agentViewModel = viewModel.Flatten().OfType<AgentViewModel>()
                                                     .FirstOrDefault(m => m.Name == agentName);
            agentViewModel.Should().NotBeNull($"the message should be in the view model:{Environment.NewLine}" +
                                              string.Join(Environment.NewLine, viewModel.Flatten()));
            scenarioContext.Get<IMessageBoard>().Publish(new SelectedTreeViewItemChanged(agentViewModel,scenarioContext.Get<InitializeMessage>()));
            scenarioContext.WaitForSilentPulse();
            
            switch (type)
            {
                case "consumed":
                    agentViewModel.NewConsumingMessage = name;
                    break;
                case "produced":
                    agentViewModel.NewProducingMessage = name;
                    break;
                case "intercepted":
                    agentViewModel.NewInterceptingMessage = name;
                    break;
            }
        }
    }
}
