#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agents.Net.Designer.Model.Messages;
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
    }
}
