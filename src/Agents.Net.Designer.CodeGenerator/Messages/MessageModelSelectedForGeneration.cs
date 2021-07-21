#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class MessageModelSelectedForGeneration : MessageDecorator
    {
        private MessageModelSelectedForGeneration(MessageModel message, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Message = message;
        }

        public static MessageModelSelectedForGeneration Decorate(ModelSelectedForGeneration message, MessageModel model)
        {
            return new(model, message);
        }

        public MessageModel Message { get; }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
