#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class CommunityModel
    {
        public CommunityModel(GeneratorSettings generatorSettings = null, AgentModel[] agents = null, MessageModel[] messages = null)
        {
            GeneratorSettings = generatorSettings??new GeneratorSettings();
            Agents = agents ?? new AgentModel[0];
            Messages = messages?? new MessageModel[0];
        }

        public GeneratorSettings GeneratorSettings { get; }

        public AgentModel[] Agents { get; }

        public MessageModel[] Messages { get; }

        public override string ToString()
        {
            return $"{nameof(Agents)}: {Agents.Length}; {nameof(Messages)}: {Messages.Length}; {nameof(GeneratorSettings)}: {GeneratorSettings}";
        }
    }
}
