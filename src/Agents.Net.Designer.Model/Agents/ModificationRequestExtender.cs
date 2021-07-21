#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Linq;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Intercepts(typeof(ModificationRequest))]
    [Consumes(typeof(ModificationRequestExtended))]
    [Consumes(typeof(ExceptionMessage))]
    [Produces(typeof(ModificationRequestExtending))]
    public class ModificationRequestExtender : InterceptorAgent
    {
        private readonly MessageGate<ModificationRequestExtending, ModificationRequestExtended> gate = new();
        public ModificationRequestExtender(IMessageBoard messageBoard, string name = null)
            : base(messageBoard, name)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            gate.Check(messageData);
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            if (messageData.Is<ExtendedModificationRequest>())
            {
                return InterceptionAction.Continue;
            }
            gate.SendAndContinue(new ModificationRequestExtending(messageData, messageData.Get<ModificationRequest>().Modifications), OnMessage,
                                 result =>
                                 {
                                     if (result.Result != MessageGateResultKind.Success)
                                     {
                                         return;
                                     }
                                     
                                     OnMessage(ExtendedModificationRequest.Decorate(new ModificationRequest(result.EndMessage, result.EndMessage.Modifications.ToArray())));
                                 });
            return InterceptionAction.DoNotPublish;
        }
    }
}