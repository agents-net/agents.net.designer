using System;
using System.Threading;
using Agents.Net;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools.Agents
{
    [Consumes(typeof(InitializeMessage))]
    public class InitializeMessageCatcher : Agent
    {
        private readonly ScenarioContext scenarioContext;

        public InitializeMessageCatcher(IMessageBoard messageBoard, ScenarioContext scenarioContext) : base(messageBoard)
        {
            this.scenarioContext = scenarioContext;
        }

        protected override void ExecuteCore(Message messageData)
        {
            scenarioContext.Set(messageData.Get<InitializeMessage>());
            scenarioContext.Get<ManualResetEventSlim>(StringConstants.InitializedMessageReceived).Set();
        }
    }
}
