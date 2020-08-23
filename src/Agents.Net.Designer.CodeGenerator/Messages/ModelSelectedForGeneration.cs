using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Messages
{
    public class ModelSelectedForGeneration : Message
    {        public ModelSelectedForGeneration(string generationPath, Message predecessorMessage,
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
