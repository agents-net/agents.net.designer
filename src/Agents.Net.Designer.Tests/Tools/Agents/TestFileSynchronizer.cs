using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools.Agents
{
    [Consumes(typeof(FileSynchronized))]
    public class TestFileSynchronizer : Agent
    {
        private readonly ScenarioContext scenarioContext;

        public TestFileSynchronizer(IMessageBoard messageBoard, ScenarioContext scenarioContext) : base(messageBoard)
        {
            this.scenarioContext = scenarioContext;
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (!scenarioContext.TryGetValue(StringConstants.FileUpdated, out List<string> updatedFiles))
            {
                updatedFiles = new List<string>();
                scenarioContext.Set(updatedFiles, StringConstants.FileUpdated);
            }
            updatedFiles.Add(messageData.Get<FileSynchronized>().FileName);
        }
    }
}
