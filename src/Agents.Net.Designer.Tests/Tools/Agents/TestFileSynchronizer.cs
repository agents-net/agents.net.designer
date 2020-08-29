using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Serialization.Messages;
using NLog;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools.Agents
{
    [Intercepts(typeof(JsonTextUpdated))]
    [Consumes(typeof(FileConnected))]
    public class TestFileSynchronizer : InterceptorAgent
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly ScenarioContext scenarioContext;
        private readonly MessageCollector<FileConnected, JsonTextUpdated> collector
            = new MessageCollector<FileConnected, JsonTextUpdated>();

        public TestFileSynchronizer(IMessageBoard messageBoard, ScenarioContext scenarioContext) : base(messageBoard)
        {
            this.scenarioContext = scenarioContext;
        }

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            collector.Push(messageData);
            MessageCollection<FileConnected, JsonTextUpdated> set = collector.FindSetsForDomain(messageData.MessageDomain).First();
            if (!scenarioContext.TryGetValue(StringConstants.UpdatedFileContents, out Dictionary<string, string> updatedFileContents))
            {
                updatedFileContents = new Dictionary<string, string>();
                scenarioContext.Set(updatedFileContents, StringConstants.UpdatedFileContents);
            }

            updatedFileContents[set.Message1.FileName] = set.Message2.Text;
            Logger.Info(set.Message2.Text);
            return InterceptionAction.DoNotPublish;
        }
    }
}
