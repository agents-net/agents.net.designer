using System;
using Agents.Net;
using Agents.Net.Designer.ViewModel.Messages;

namespace Agents.Net.Designer.ViewModel.Agents
{
    public class JsonViewModelCreator : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition JsonViewModelCreatorDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      InitializeMessage.InitializeMessageDefinition
                                  },
                                  new []
                                  {
                                      JsonViewModelCreated.JsonViewModelCreatedDefinition
                                  });

        #endregion

        public JsonViewModelCreator(IMessageBoard messageBoard) : base(JsonViewModelCreatorDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            OnMessage(new JsonViewModelCreated(new JsonViewModel(), messageData));
        }
    }
}
