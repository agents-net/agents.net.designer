#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Consumes(typeof(MessageModelSelectedForGeneration))]
    [Consumes(typeof(ModelSelectedForGeneration), Implicitly = true)]
    [Consumes(typeof(MessageDecoratorSelectedForGeneration), Implicitly = true)]
    [Produces(typeof(GeneratingMessageDecorator))]
    [Produces(typeof(GeneratingMessage))]
    [Produces(typeof(GeneratingFile))]
    public class MessageModelObjectParser : Agent
    {
        public MessageModelObjectParser(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            MessageModelSelectedForGeneration messageModel = messageData.Get<MessageModelSelectedForGeneration>();
            Message message = GeneratingMessage.Decorate(new GeneratingFile(messageModel.Message.Name,
                                                                            messageModel.Message.FullNamespace(),
                                                                            messageModel.Get<ModelSelectedForGeneration>()
                                                                                .GenerationPath, messageModel));
            if (messageModel.TryGet(out MessageDecoratorSelectedForGeneration messageDecorator))
            {
                string messageNamespace = messageDecorator.DecoratedMessage.FullNamespace();
                string dependency = messageNamespace != messageModel.Message.FullNamespace()
                                        ? messageNamespace
                                        : string.Empty;
                message = GeneratingMessageDecorator.Decorate((GeneratingMessage) message, dependency, messageDecorator.DecoratedMessage.Name);
            }
            OnMessage(message);
        }
    }
}
