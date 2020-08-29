using System;
using System.Threading;
using Agents.Net;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools.Agents
{
    [Consumes(typeof(Message))]
    public class MessagePulseAgent : Agent
    {
        private readonly ScenarioContext scenarioContext;

        public MessagePulseAgent(IMessageBoard messageBoard, ScenarioContext scenarioContext) : base(messageBoard)
        {
            this.scenarioContext = scenarioContext;
            scenarioContext.Set(new AutoResetEvent(false), StringConstants.Pulse);
        }
        
        protected override void ExecuteCore(Message messageData)
        {
            scenarioContext.Get<AutoResetEvent>(StringConstants.Pulse).Set();
        }
    }
}
