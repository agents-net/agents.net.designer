using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModifyModel))]
    [Produces(typeof(ModifyModel))]
    [Produces(typeof(ModelModificationBatch))]
    public class AutomaticMessageModelCreator : InterceptorAgent
    {
        public AutomaticMessageModelCreator(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            ModifyModel modifyModel = messageData.Get<ModifyModel>();
            if (messageData.Is<ModelModificationBatch>() ||
                modifyModel.ModificationType != ModelModification.Add ||
                modifyModel.Target is not AgentModel agentModel ||
                !(modifyModel.Property is AgentConsumingMessagesProperty ||
                  modifyModel.Property is AgentProducedMessagesProperty ||
                  modifyModel.Property is InterceptorAgentInterceptingMessagesProperty) ||
                modifyModel.NewValue is Guid)
            {
                return InterceptionAction.Continue;
            }
            
            string messageDefinition = modifyModel.NewValue.AssertTypeOf<string>();
            MessageModel newMessageModel = new(
                messageDefinition.Substring(messageDefinition.LastIndexOf('.') + 1),
                messageDefinition.Contains('.')
                    ? messageDefinition.Substring(
                        0, messageDefinition.LastIndexOf('.'))
                    : ".Messages");
            ModifyModel addMessage = new(ModelModification.Add,
                                         null,
                                         newMessageModel,
                                         agentModel.ContainingPackage,
                                         new PackageMessagesProperty(),
                                         messageData);
            ModifyModel modifyMessage = new(modifyModel.ModificationType,
                                            modifyModel.OldValue,
                                            newMessageModel.Id,
                                            modifyModel.Target, modifyModel.Property,
                                            messageData);
            OnMessage(ModelModificationBatch.Create(new []{addMessage, modifyMessage}));
            return InterceptionAction.DoNotPublish;
        }
    }
}
