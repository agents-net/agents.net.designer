using System;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;
using Microsoft.Msagl.Drawing;

namespace Agents.Net.Designer.ViewModel.Agents
{
    public class SelectedObjectTranslator : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition SelectedObjectTranslatorDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      SelectedObjectChanged.SelectedObjectChangedDefinition
                                  },
                                  new []
                                  {
                                      SelectedModelObjectChanged.SelectedModelObjectChangedDefinition
                                  });

        #endregion

        public SelectedObjectTranslator(IMessageBoard messageBoard) : base(SelectedObjectTranslatorDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            SelectedObjectChanged selectedObjectChanged = messageData.Get<SelectedObjectChanged>();
            if (selectedObjectChanged.SelectedObject is Node node)
            {
                OnMessage(new SelectedModelObjectChanged(node.UserData, messageData));
            }
        }
    }
}
