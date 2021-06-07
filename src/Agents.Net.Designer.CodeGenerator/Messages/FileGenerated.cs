using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class FileGenerated : Message
    {
        public FileGenerated(FileGenerationResult result, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Result = result;
        }

        public FileGenerated(FileGenerationResult result, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            Result = result;
        }

        public FileGenerationResult Result { get; }

        protected override string DataToString()
        {
            return $"{nameof(Result)}: {Result}";
        }
    }
}
