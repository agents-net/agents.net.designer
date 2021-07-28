#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Linq;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Tests.Tools;
using Agents.Net.Designer.ViewModel;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.StepDefinitions
{
    public partial class GraphStepDefinitions
    {
        [When(@"I switch the graph view scope to ""(.*)""")]
        public void WhenISwitchTheGraphViewScopeTo(string newScope)
        {
            Enum.TryParse(newScope, true, out GraphViewScope scope)
                .Should().BeTrue();
            scenarioContext.WaitForSilentPulse();
            GraphViewModel viewModel = scenarioContext.Get<GraphViewModel>(StringConstants.GraphViewModelCreated);

            viewModel.Scope = scope;
        }
    }
}