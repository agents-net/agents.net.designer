using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agents.Net.Designer.Tests.Tools;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.StepDefinitions
{
    [Binding]
    public sealed class SerializationStepDefinitions
    {
        // For additional details on SpecFlow step definitions see https://go.specflow.org/doc-stepdef

        private readonly ScenarioContext scenarioContext;

        public SerializationStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Then(@"the model file ""(.*)"" looks like the file ""(.*)""")]
        public void ThenTheModelFileLooksLikeTheFile(string fileName, string resourceContent)
        {
            scenarioContext.WaitForSilentPulse();
            scenarioContext.TryGetValue(StringConstants.UpdatedFileContents,
                                        out Dictionary<string, string> fileContents)
                           .Should().BeTrue("otherwise no file was updated.");
            fileContents.TryGetValue(fileName, out string content).Should().BeTrue(
                $"otherwise the file {fileName} was not updated." +
                $"Updated files are {string.Join(", ", fileContents.Keys)}");
            string expectedContent = resourceContent.GetResourceContent();
            StripIds(content).Should().Be(StripIds(expectedContent));
        }

        private string StripIds(string json)
        {
            JObject root = JObject.Parse(json);
            Stack<JToken> unvisited = new Stack<JToken>(root.Children());
            while (unvisited.Any())
            {
                JToken current = unvisited.Pop();
                if (current is JProperty property)
                {
                    if (property.Name.Equals("id", StringComparison.OrdinalIgnoreCase))
                    {
                        property.Remove();
                    }
                }
                foreach (JToken child in current.Children())
                {
                    unvisited.Push(child);
                }
            }

            return root.ToString(Formatting.Indented);
        }

    }
}
