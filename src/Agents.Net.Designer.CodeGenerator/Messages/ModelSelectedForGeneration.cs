#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class ModelSelectedForGeneration : Message
    {
        public ModelSelectedForGeneration(string generationPath, Message predecessorMessage)
            : base(predecessorMessage)
        {
            GenerationPath = generationPath;
        }

        public ModelSelectedForGeneration(string generationPath,
                                          IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
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
