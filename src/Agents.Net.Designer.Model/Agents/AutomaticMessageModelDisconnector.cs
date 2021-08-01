#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Model.Messages;
using ConcurrentCollections;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModificationRequestExtending))]
    public class AutomaticMessageModelDisconnector : InterceptorAgent
    {
        public AutomaticMessageModelDisconnector(IMessageBoard messageBoard, string name = null)
            : base(messageBoard, name)
        {
        }

        private readonly ConcurrentHashSet<Modification> checkedModifications = new();

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
                if (modification.ModificationType != ModificationType.Remove ||
                    modification.Target is not CommunityModel model ||
                    modification.Property is not PackageMessagesProperty ||
                    modification.OldValue is not MessageModel deletedModel)
                {
                    continue;
                }

                if (!checkedModifications.Add(modification))
                {
                    continue;
                }
                
                List<Modification> disconnections = new();
                foreach (AgentModel agent in model.Agents.Where(a => a.ConsumingMessages.Contains(deletedModel.Id)))
                {
                    disconnections.Add(new Modification(ModificationType.Remove, deletedModel.Id, null, agent, new AgentConsumingMessagesProperty()));
                }
                foreach (AgentModel agent in model.Agents.Where(a => a.ProducedMessages.Contains(deletedModel.Id)))
                {
                    disconnections.Add(new Modification(ModificationType.Remove, deletedModel.Id, null, agent, new AgentProducedMessagesProperty()));
                }
                foreach (InterceptorAgentModel agent in model.Agents.OfType<InterceptorAgentModel>().Where(a => a.InterceptingMessages.Contains(deletedModel.Id)))
                {
                    disconnections.Add(new Modification(ModificationType.Remove, deletedModel.Id, null, agent, new InterceptorAgentInterceptingMessagesProperty()));
                }

                foreach (MessageDecoratorModel message in model.Messages.OfType<MessageDecoratorModel>().Where(m => m.DecoratedMessage == deletedModel.Id))
                {
                    disconnections.Add(new Modification(ModificationType.Change, deletedModel.Id, null, message, new MessageDecoratorDecoratedMessageProperty()));
                }
                modifications.InsertRange(modifications.IndexOf(modification), disconnections);
                modified = true;
            }

            return modified;
        }
    }
}