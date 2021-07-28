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
    [Binding]
    public partial class GraphStepDefinitions
    {
        private readonly ScenarioContext scenarioContext;

        public GraphStepDefinitions(ScenarioContext scenarioContext)
        {
            this.scenarioContext = scenarioContext;
        }

        [Then(@"the scope of the graph is ""(.*)""")]
        public void ThenTheScopeOfTheGraphIs(string scope)
        {
            Enum.TryParse(scope, true, out GraphViewScope expected)
                .Should().BeTrue();
            scenarioContext.WaitForSilentPulse();
            GraphViewModel viewModel = scenarioContext.Get<GraphViewModel>(StringConstants.GraphViewModelCreated);

            viewModel.Scope.Should().Be(expected);
        }

        [Then(@"the graph contains the interceptor agent ""(.*)""")]
        public void ThenTheGraphContainsTheInterceptorAgent(string name)
        {
            scenarioContext.WaitForSilentPulse();
            
            GraphViewModel viewModel = scenarioContext.Get<GraphViewModel>(StringConstants.GraphViewModelCreated);
            InterceptorAgentModel model = viewModel.Graph.Nodes.Select(n => n.UserData)
                                                   .OfType<InterceptorAgentModel>()
                                                   .FirstOrDefault(i => i.Name == name);
            model.Should()
                 .NotBeNull(
                     $"the interceptor {name} was expected to exist in the graph. Available nodes are:{Environment.NewLine}{string.Join(Environment.NewLine, viewModel.Graph.Nodes.Select(n => n.UserData))}");
        }

        [Then(@"the graph (does not show|shows) the following objects:")]
        public void ThenTheGraphShowsTheFollowingObjects(string shows, Table objects)
        {
            bool isVisible = shows == "shows";
            scenarioContext.WaitForSilentPulse();
            GraphViewModel viewModel = scenarioContext.Get<GraphViewModel>(StringConstants.GraphViewModelCreated);
            
            foreach (TableRow row in objects.Rows)
            {
                string name = row["DisplayName"];
                if (isVisible)
                {
                    viewModel.Graph.Nodes.Should().Contain(n => n.LabelText == name);
                }
                else
                {
                    viewModel.Graph.Nodes.Should().NotContain(n => n.LabelText == name);
                }
            }
        }
    }
}