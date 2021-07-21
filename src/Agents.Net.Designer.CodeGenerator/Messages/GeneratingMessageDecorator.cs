#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class GeneratingMessageDecorator : MessageDecorator
    {
        public GeneratingMessageDecorator(string dependency, string decoratedMessageName, Message decoratedMessage)
            : base(decoratedMessage)
        {
            Dependency = dependency;
            DecoratedMessageName = decoratedMessageName;
        }

        public string Dependency { get; }

        public string DecoratedMessageName { get; }

        public static GeneratingMessageDecorator Decorate(GeneratingMessage message, string dependency,
                                                          string decoratedMessageName)
        {
            return new(dependency, decoratedMessageName, message);
        }

        protected override string DataToString()
        {
            return $"{nameof(DecoratedMessageName)}: {DecoratedMessageName}; {nameof(Dependency)}:{Dependency}";
        }
    }
}
