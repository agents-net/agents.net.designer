#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using Agents.Net.Designer.ViewModel.Messages;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools.Agents
{
    [Consumes(typeof(TreeViewModelCreated))]
    [Consumes(typeof(GraphViewModelCreated))]
    public class InformationCollector : Agent
    {
        private readonly ScenarioContext scenarioContext;
        public InformationCollector(IMessageBoard messageBoard, ScenarioContext scenarioContext, string name = null)
            : base(messageBoard, name)
        {
            this.scenarioContext = scenarioContext;
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (messageData.TryGet(out GraphViewModelCreated graphCreated))
            {
                scenarioContext.Set(graphCreated.ViewModel, StringConstants.GraphViewModelCreated);
            }
            else
            {
                TreeViewModelCreated viewModelCreated = messageData.Get<TreeViewModelCreated>();
                scenarioContext.Set(viewModelCreated.ViewModel, StringConstants.TreeViewModelCreated);
            }
        }
    }
}