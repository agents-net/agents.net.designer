#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModificationRequestExtending))]
    public class AutomaticMessageModelCreator : InterceptorAgent
    {
        public AutomaticMessageModelCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            messageData.Get<ModificationRequestExtending>().RegisterExtender(ExtendModifications);
            return InterceptionAction.Continue;
        }

        private bool ExtendModifications(List<Modification> modifications)
        {
            bool modified = false;
            foreach (Modification modification in modifications.ToArray())
            {
                if (modification.ModificationType != ModificationType.Add ||
                    modification.Target is not AgentModel agentModel ||
                    !(modification.Property is AgentConsumingMessagesProperty ||
                      modification.Property is AgentProducedMessagesProperty ||
                      modification.Property is InterceptorAgentInterceptingMessagesProperty) ||
                    modification.NewValue is not string)
                {
                    continue;
                }
            
                string messageDefinition = modification.NewValue.AssertTypeOf<string>();
                MessageModel newMessageModel = new(
                    GetName(messageDefinition),
                    GetNamespace(messageDefinition, agentModel));
                Modification addMessage = new(ModificationType.Add,
                                              null,
                                              newMessageModel,
                                              agentModel.ContainingPackage,
                                              new PackageMessagesProperty());
                Modification modifyMessage = new(modification.ModificationType,
                                                 modification.OldValue,
                                                 newMessageModel.Id,
                                                 modification.Target, modification.Property);
                modifications.InsertRange(modifications.IndexOf(modification),
                                          new[] {addMessage, modifyMessage});
                modifications.Remove(modification);
                modified = true;
            }

            return modified;

            string GetName(string messageDefinition)
            {
                return messageDefinition.Substring(messageDefinition.LastIndexOf('.') + 1);
            }

            string GetNamespace(string messageDefinition, AgentModel agentModel)
            {
                return messageDefinition.Contains('.')
                           ? messageDefinition.Substring(
                               0, messageDefinition.LastIndexOf('.'))
                           : GetAgentMessageNamespace(agentModel);
            }
        }

        private string GetAgentMessageNamespace(AgentModel agentModel)
        {
            string agentNamespace = agentModel.Namespace;
            if (agentNamespace.EndsWith(".Agents", StringComparison.Ordinal))
            {
                return agentNamespace.Substring(0, agentNamespace.Length - ".Agents".Length)
                       + ".Messages";
            }

            return agentNamespace;
        }
    }
}
