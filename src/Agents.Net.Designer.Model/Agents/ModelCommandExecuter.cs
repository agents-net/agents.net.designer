using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using Agents.Net;
using Agents.Net.Designer.Model.Messages;

namespace Agents.Net.Designer.Model.Agents
{
    [Consumes(typeof(AddAgentRequested))]
    [Consumes(typeof(AddMessageRequested))]
    [Consumes(typeof(AddInterceptorAgentRequested))]
    [Consumes(typeof(AddMessageDecoratorRequested))]
    [Consumes(typeof(ModelVersionCreated))]
    [Produces(typeof(ModifyModel))]
    public class ModelCommandExecuter : Agent
    {
        private readonly MessageCollector<AddAgentRequested, ModelVersionCreated> addAgentCollector;
        private readonly MessageCollector<AddMessageRequested, ModelVersionCreated> addMessageCollector;
        private readonly MessageCollector<AddInterceptorAgentRequested, ModelVersionCreated> addInterceptorCollector;
        private readonly MessageCollector<AddMessageDecoratorRequested, ModelVersionCreated> addDecoratorCollector;

        public ModelCommandExecuter(IMessageBoard messageBoard) : base(messageBoard)
        {
            addAgentCollector = new MessageCollector<AddAgentRequested, ModelVersionCreated>(OnMessagesCollected);
            addMessageCollector = new MessageCollector<AddMessageRequested, ModelVersionCreated>(OnMessagesCollected);
            addInterceptorCollector = new MessageCollector<AddInterceptorAgentRequested, ModelVersionCreated>(OnMessagesCollected);
            addDecoratorCollector = new MessageCollector<AddMessageDecoratorRequested, ModelVersionCreated>(OnMessagesCollected);
        }

        private void OnMessagesCollected(MessageCollection<AddMessageDecoratorRequested, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new MessageDecoratorModel("MessageDecoratorX"), set.Message2.Model,
                                      new PackageMessagesProperty(), set));
        }

        private void OnMessagesCollected(MessageCollection<AddInterceptorAgentRequested, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new InterceptorAgentModel("InterceptorX"), set.Message2.Model,
                                      new PackageAgentsProperty(), set));
        }

        private void OnMessagesCollected(MessageCollection<AddMessageRequested, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new MessageModel("MessageX"), set.Message2.Model,
                                      new PackageMessagesProperty(), set));
        }

        private void OnMessagesCollected(MessageCollection<AddAgentRequested, ModelVersionCreated> set)
        {
            set.MarkAsConsumed(set.Message1);

            OnMessage(new ModifyModel(ModelModification.Add,
                                      null, new AgentModel("AgentX"), set.Message2.Model,
                                      new PackageAgentsProperty(), set));
        }

        protected override void ExecuteCore(Message messageData)
        {
            addAgentCollector.TryPush(messageData);
            addMessageCollector.TryPush(messageData);
            addInterceptorCollector.TryPush(messageData);
            addDecoratorCollector.TryPush(messageData);
        }
    }
}
