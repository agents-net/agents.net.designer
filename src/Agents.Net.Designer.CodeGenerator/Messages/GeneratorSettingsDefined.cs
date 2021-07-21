#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using Agents.Net.Designer.Model;

namespace Agents.Net.Designer.CodeGenerator.Messages
{
    public class GeneratorSettingsDefined : Message
    {
        public GeneratorSettingsDefined(GeneratorSettings settings, string path, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Settings = settings;
            Path = path;
        }

        public GeneratorSettingsDefined(GeneratorSettings settings, string path,
                                        IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            Settings = settings;
            Path = path;
        }

        public GeneratorSettings Settings { get; }
        public string Path { get; }

        protected override string DataToString()
        {
            return $"{nameof(Settings)}: {Settings}, {nameof(Path)}: {Path}";
        }
    }
}
