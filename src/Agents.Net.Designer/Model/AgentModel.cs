using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Model
{
    public class AgentModel
    {
        public AgentModel(string name = "", string @namespace = ".Agents", Guid[] consumingMessages = null,
                          Guid[] producedMessages = null, string[] incomingEvents = null, string[] producedEvents = null,
                          Guid id = default)
        {
            Name = name;
            Namespace = @namespace;
            ConsumingMessages = consumingMessages ?? new Guid[0];
            ProducedMessages = producedMessages ?? new Guid[0];
            IncomingEvents = incomingEvents ?? new string[0];
            ProducedEvents = producedEvents ?? new string[0];
            Id = id == default ? Guid.NewGuid() : id;
        }
        
        public Guid Id { get; }

        public string Name { get; }

        public string Namespace { get; }

        public Guid[] ConsumingMessages { get; }

        public Guid[] ProducedMessages { get; }

        public string[] IncomingEvents { get; }
        
        public string[] ProducedEvents { get; }
    }
}
