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
    public sealed partial class GeneralStepDefinitions
    {
        [When(@"I generate all classes")]
        public void WhenIGenerateAllClasses()
        {
            scenarioContext.Get<IMessageBoard>().Publish(new GenerateFilesRequested(string.Empty, scenarioContext.Get<InitializeMessage>()));
        }
    }
}
