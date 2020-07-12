using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Json.Messages;
using Agents.Net.Designer.Model;
using Agents.Net.Designer.Model.Messages;
using Agents.Net.Designer.ViewModel.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Agents.Net.Designer.ViewModel.Agents
{
    public class SelectedModelObjectToSelectedTextPosition : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition SelectedModelObjectToSelectedTextPositionDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      JsonModelValidated.JsonModelValidatedDefinition,
                                      SelectedModelObjectChanged.SelectedModelObjectChangedDefinition
                                  },
                                  new []
                                  {
                                      SelectedJsonPositionChanged.SelectedJsonPositionChangedDefinition
                                  });

        #endregion

        public SelectedModelObjectToSelectedTextPosition(IMessageBoard messageBoard) : base(SelectedModelObjectToSelectedTextPositionDefinition, messageBoard)
        {
            collector = new MessageCollector<JsonModelValidated, SelectedModelObjectChanged>(OnMessagesCollected);
        }

        private readonly HashSet<SelectedModelObjectChanged> processedMessages = new HashSet<SelectedModelObjectChanged>();

        private void OnMessagesCollected(MessageCollection<JsonModelValidated, SelectedModelObjectChanged> set)
        {
            lock (processedMessages)
            {
                if (!processedMessages.Add(set.Message2))
                {
                    return;
                }
            }
            JToken token;
            switch (set.Message2.SelectedObject)
            {
                case AgentModel agentModel:
                    token = set.Message1.ValidatedModel["Agents"]
                               .First(a => a.Value<string>("Name") == agentModel.Name);
                    break;
                case MessageModel messageModel:
                    token = set.Message1.ValidatedModel["Messages"]
                               .First(a => a.Value<string>("Name") == messageModel.Name);
                    break;
                default:
                    throw new InvalidOperationException("Impossible");
            }

            IJsonLineInfo lineInfo = token;
            int endLine = -1, endColumn = -1;
            while (token.Next == null && token.Parent != null)
            {
                token = token.Parent;
            }
            if (token.Next != null)
            {
                endLine = ((IJsonLineInfo) token.Next).LineNumber;
                endColumn = ((IJsonLineInfo) token.Next).LinePosition;
            }
            OnMessage(new SelectedJsonPositionChanged(lineInfo.LineNumber, lineInfo.LinePosition, endLine, endColumn, set));
        }

        private readonly MessageCollector<JsonModelValidated, SelectedModelObjectChanged> collector;

        protected override void ExecuteCore(Message messageData)
        {
            collector.Push(messageData);
        }
    }
}
