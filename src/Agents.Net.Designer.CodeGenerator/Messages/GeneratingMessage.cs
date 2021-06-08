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
