using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Model
{
    public class CommunityModel
    {
        public CommunityModel(GeneratorSettings generatorSettings = null, AgentModel[] agents = null, MessageModel[] messages = null)
        {
            GeneratorSettings = generatorSettings;
            Agents = agents ?? new AgentModel[0];
            Messages = messages?? new MessageModel[0];
        }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public GeneratorSettings GeneratorSettings { get; }

        public AgentModel[] Agents { get; }

        public MessageModel[] Messages { get; }
    }
}
