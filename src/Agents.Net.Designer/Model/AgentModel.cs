using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Model
{
    public class AgentModel
    {
        public AgentModel(string name = "", string @namespace = ".Agents", string[] consumingMessages = null,
                          string[] producedMessages = null, string[] incomingEvents = null, string[] producedEvents = null,
                          Guid id = default)
        {
            Name = name;
            Namespace = @namespace;
            ConsumingMessages = consumingMessages ?? new string[0];
            ProducedMessages = producedMessages ?? new string[0];
            IncomingEvents = incomingEvents;
            ProducedEvents = producedEvents;
            Id = id == default ? Guid.NewGuid() : id;
        }
        
        [JsonIgnore]
        public Guid Id { get; }

        public string Name { get; }

        public string Namespace { get; }

        public string[] ConsumingMessages { get; }

        public string[] ProducedMessages { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] IncomingEvents { get; }
        
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string[] ProducedEvents { get; }
    }
}
