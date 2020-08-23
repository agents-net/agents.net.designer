using System.Collections.Generic;
using Agents.Net;

namespace Agents.Net.Designer.Generator.Messages
{
    public class GeneratingMessageDecorator : Message
    {
        public GeneratingMessageDecorator(string dependency, string decoratedMessageName, Message predecessorMessage,
                                          params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            Dependency = dependency;
            DecoratedMessageName = decoratedMessageName;
        }

        public GeneratingMessageDecorator(string dependency, string decoratedMessageName,
                                          IEnumerable<Message> predecessorMessages,
                                          params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            Dependency = dependency;
            DecoratedMessageName = decoratedMessageName;
        }

        public string Dependency { get; }

        public string DecoratedMessageName { get; }

        protected override string DataToString()
        {
            return $"{nameof(DecoratedMessageName)}: {DecoratedMessageName}; {nameof(Dependency)}:{Dependency}";
        }
    }
}
