#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(ModificationRequest))]
    [Consumes(typeof(ModelModified))]
    [Consumes(typeof(ExceptionMessage))]
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(ModifyModel))]
    [Produces(typeof(ModificationCompleted))]
    public class ModificationBatchExecuter : Agent
    {
        private readonly MessageGate<ModifyModel, ModelModified> gate = new();
        private readonly MessageCollector<ModificationRequest, ModelVersionCreated> collector;
        public ModificationBatchExecuter(IMessageBoard messageBoard)
            : base(messageBoard)
        {
            collector = new MessageCollector<ModificationRequest, ModelVersionCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<ModificationRequest, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);
            set.MarkAsConsumed(set.Message2);
            
            ModificationRequest request = set.Message1;
            Queue<Modification> remainingModifications = new(request.Modifications);
            SendNextModification(set.ToArray(), set.Message2.Model);
            
            void SendNextModification(IReadOnlyCollection<Message> predecessors, CommunityModel communityModel)
            {
                if (remainingModifications.Count == 0)
                {
                    ModelModified modelModified = predecessors.OfType<ModelModified>()
                                                              .FirstOrDefault();
                    if (modelModified != null)
                    {
                        OnMessage(new ModificationCompleted(predecessors, modelModified.Model));
                    }
                    return;
                }

                bool isLast = remainingModifications.Count == 1;
                Modification next = remainingModifications.Dequeue();
                ModifyModel modifyModel = new(predecessors, next, isLast, communityModel);
                gate.SendAndContinue(modifyModel, OnMessage, result =>
                {
                    if (result.Result == MessageGateResultKind.Success)
                    {
                        SendNextModification(new []{result.EndMessage}, result.EndMessage.Model);
                    }
                });
            }
        }

        protected override void ExecuteCore(Message messageData)
        {
            gate.Check(messageData);
            collector.TryPush(messageData);
        }
    }
}