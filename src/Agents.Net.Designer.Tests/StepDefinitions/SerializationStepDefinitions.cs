#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Agents.Net.Designer.Tests.Tools;
using Agents.Net.Designer.Tests.Tools.Agents;
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
            string content = scenarioContext.Get<FileSystemSimulator>().GetFileContent(fileName);
            string expectedContent = resourceContent.GetResourceContent();
            StripIds(content).Should().Be(StripIds(expectedContent));
        }

        private string StripIds(string json)
        {
            JObject root = JObject.Parse(json);
            Stack<JToken> unvisited = new(root.Children());
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
