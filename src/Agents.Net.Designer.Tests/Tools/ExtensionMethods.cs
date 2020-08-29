using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools
{
    internal static class ExtensionMethods
    {
        public static void WaitForSilentPulse(this ScenarioContext scenarioContext, int timeout = 300)
        {
            AutoResetEvent pulseEvent = scenarioContext.Get<AutoResetEvent>(StringConstants.Pulse);
            while (pulseEvent.WaitOne(timeout))
            {
                //do nothing
            }
        }

        public static string GetResourceContent(this string fileName)
        {
            string resourceKey =
                $"Agents.Net.Designer.Tests.Deployment.{string.Join('.', fileName.Split(new[] {'/', '\\'}, StringSplitOptions.RemoveEmptyEntries))}";
            using Stream resourceStream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceKey);
            resourceStream.Should().NotBeNull($"resource {resourceKey} was expected to exist.");
            using StreamReader reader = new StreamReader(resourceStream, Encoding.UTF8, true);
            string resourceContent = reader.ReadToEnd();
            return resourceContent;
        }
    }
}
