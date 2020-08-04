using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;
using AngleSharp.Common;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModifyModel))]
    [Consumes(typeof(ModelUpdated))]
    [Produces(typeof(ModifyModel))]
    public class AutomaticMessageModelCreator : InterceptorAgent
    {
        public AutomaticMessageModelCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        private readonly ConcurrentDictionary<ModifyModel, ModifyModel> originalModifications = new ConcurrentDictionary<ModifyModel, ModifyModel>();

        protected override void ExecuteCore(Message messageData)
        {
            if (!messageData.TryGetPredecessor(out ModifyModel lastModification) ||
                !originalModifications.TryGetValue(lastModification, out ModifyModel originalModification))
            {
                return;
            }

            Guid messageId = ((MessageModel)lastModification.NewValue).Id;
            OnMessage(new ModifyModel(originalModification.ModificationType,
                                      originalModification.OldValue,
                                      messageId,
                                      originalModification.Target, originalModification.Property,
                                      messageData));
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            ModifyModel modifyModel = messageData.Get<ModifyModel>();
            if (modifyModel.ModificationType != ModelModification.Add ||
                !(modifyModel.Target is AgentModel agentModel) ||
                !(modifyModel.Property is AgentConsumingMessagesProperty ||
                  modifyModel.Property is AgentProducedMessagesProperty) ||
                modifyModel.NewValue is Guid)
            {
                return InterceptionAction.Continue;
            }
            
            string messageDefinition = modifyModel.NewValue.AssertTypeOf<string>();
            ModifyModel addMessage = new ModifyModel(ModelModification.Add,
                                                     null,
                                                     new MessageModel(
                                                         messageDefinition.Substring(messageDefinition.LastIndexOf('.') + 1),
                                                         messageDefinition.Contains('.')
                                                             ? messageDefinition.Substring(
                                                                 0, messageDefinition.LastIndexOf('.'))
                                                             : ".Messages"),
                                                     agentModel.ContainingPackage,
                                                     new PackageMessagesProperty(),
                                                     messageData);
            originalModifications.TryAdd(addMessage, modifyModel);
            OnMessage(addMessage);
            return InterceptionAction.DoNotPublish;
        }
    }
}
