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
    }
}
