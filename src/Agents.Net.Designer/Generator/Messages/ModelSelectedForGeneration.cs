using System.Collections.Generic;
using Agents.Net;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.Generator.Messages
{
    public class ModelSelectedForGeneration : Message
    {        public ModelSelectedForGeneration(string generationPath, CommunityModel model, Message predecessorMessage,
                                          params Message[] childMessages)
            : base(predecessorMessage, childMessages:childMessages)
        {
            GenerationPath = generationPath;
            Model = model;
        }

        public ModelSelectedForGeneration(string generationPath, CommunityModel model,
                                          IEnumerable<Message> predecessorMessages, params Message[] childMessages)
            : base(predecessorMessages, childMessages:childMessages)
        {
            GenerationPath = generationPath;
            Model = model;
        }

        public string GenerationPath { get; }

        public CommunityModel Model { get; }

        protected override string DataToString()
        {
            return $"{nameof(GenerationPath)}: {GenerationPath}";
        }
    }
}
