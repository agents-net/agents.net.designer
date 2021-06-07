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
using Agents.Net.Designer.Tests.Tools.Modules;
using Autofac;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.StepDefinitions
{
    public sealed partial class GeneralStepDefinitions
    {
        [Then(@"the file ""(.*)"" was updated")]
        public void ThenTheFileWasUpdated(string fileName)
        {
            scenarioContext.WaitForSilentPulse();
            scenarioContext.TryGetValue(StringConstants.UpdatedFileContents,
                                        out Dictionary<string, string> fileContents)
                           .Should().BeTrue("otherwise no file was updated.");
            fileContents.ContainsKey(fileName).Should().BeTrue($"otherwise the file {fileName} was not updated. " +
                                                               $"Updated files are {string.Join(", ", fileContents.Keys)}");
        }

        [Then(@"the file ""(.*)"" looks like the file ""(.*)""")]
        public void ThenTheFileLooksLikeTheFile(string fileName, string resourceContent)
        {
            scenarioContext.WaitForSilentPulse();
            scenarioContext.TryGetValue(StringConstants.UpdatedFileContents,
                                        out Dictionary<string, string> fileContents)
                           .Should().BeTrue("otherwise no file was updated.");
            fileContents.TryGetValue(fileName, out string content).Should().BeTrue(
                $"otherwise the file {fileName} was not updated." +
                $"Updated files are {string.Join(", ", fileContents.Keys)}");
            string expectedContent = resourceContent.GetResourceContent();
            content.Should().Be(expectedContent);
        }
        
    }
}
