using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class AgentModel
    {
        public string Name { get; set; }

        public string Namespace { get; set; } = ".Agents";

        public string[] ConsumingMessages { get; set; } = new string[0];

        public string[] ProducingMessages { get; set; } = new string[0];

        public AgentModel Clone()
        {
            return new AgentModel
            {
                Name = Name,
                Namespace = Namespace,
                ConsumingMessages = ConsumingMessages,
                ProducingMessages = ProducingMessages
            };
        }
    }
}
