#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using Agents.Net.Designer.FileSystem.Messages;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.Serialization.Messages;
using Agents.Net.Designer.Tests.Tools;
using Agents.Net.Designer.Tests.Tools.Agents;
using Agents.Net.Designer.Tests.Tools.Modules;
using Autofac;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.StepDefinitions
{
    [Binding]
    public sealed partial class GeneralStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext scenarioContext;

        public GeneralStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Given("I have started the designer")]
        public void GivenIHaveStartedTheDesigner()
        {
            Start<CompleteModule>();
        }
        
        [Given(@"I connected the file ""(.*)""")]
        public void GivenIConnectedTheFile(string fileName)
        {
            string resourceContent = fileName.GetResourceContent();
            scenarioContext.Get<FileSystemSimulator>().SetFileContent(fileName,
                                                                      resourceContent);
            scenarioContext.Get<IMessageBoard>().Publish(new FileConnectionVerified(fileName, true,scenarioContext.Get<InitializeMessage>()));
            //Wait for model loaded
            scenarioContext.WaitForSilentPulse();
        }

        private void Start<T>()
            where T : TestBaseModule, new ()
        {
            //Create container
            using ManualResetEventSlim initializedTrigger = new(false);
            scenarioContext.Set(initializedTrigger, StringConstants.InitializedMessageReceived);
            ContainerBuilder builder = new();
            builder.RegisterModule<T>();
            builder.RegisterInstance(scenarioContext);
            IContainer container = builder.Build();

            //Start agent community
            Action startAction = () =>
            {
                IMessageBoard messageBoard = container.Resolve<IMessageBoard>();
                Agent[] agents = container.Resolve<IEnumerable<Agent>>().ToArray();
                messageBoard.Register(agents);
                messageBoard.Start();

                scenarioContext.Set(container);
                scenarioContext.Set(messageBoard);
                scenarioContext.Set(container.Resolve<FileSystemSimulator>());
            };
            startAction.Should().NotThrow<Exception>("agent community was not created.");
            initializedTrigger.Wait();
        }
    }
}
