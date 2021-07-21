#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class GeneratingMessage : MessageDecorator
    {
        private GeneratingMessage(Message decoratedMessage)
            : base(decoratedMessage)
        {
        }

        public static GeneratingMessage Decorate(GeneratingFile file)
        {
            return new(file);
        }

        protected override string DataToString()
        {
            return string.Empty;
        }
    }
}
