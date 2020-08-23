using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class ModelSelectedForGeneration : Message
    {
                                          params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            GenerationPath = generationPath;
        }

        public ModelSelectedForGeneration(string generationPath,
                                          IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            GenerationPath = generationPath;
        }

        public string GenerationPath { get; }

        protected override string DataToString()
        {
            return $"{nameof(GenerationPath)}: {GenerationPath}";
        }
    }
}