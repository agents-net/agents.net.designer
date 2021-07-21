#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class MessageDecoratorSelectedForGeneration : MessageDecorator
    {
        private MessageDecoratorSelectedForGeneration(MessageModel decoratedMessage, Message predecessorMessage)
            : base(predecessorMessage)
        {
            DecoratedMessage = decoratedMessage;
        }

        public static MessageDecoratorSelectedForGeneration Decorate(MessageModelSelectedForGeneration message,
                                                                     MessageModel model)
        {
            return new(model, message);
        }

        public MessageModel DecoratedMessage { get; }

        protected override string DataToString()
        {
            return $"{nameof(DecoratedMessage)}: {DecoratedMessage}";
        }
    }
}
