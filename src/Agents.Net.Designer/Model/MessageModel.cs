using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Model
{
    public class MessageModel
    {
        public MessageModel(string name = "", string @namespace = ".Messages",
                            Guid id = default)
        {
            Name = name;
            Namespace = @namespace;
            Id = id == default ? Guid.NewGuid() : id;
        }
        
        [JsonIgnore]
        public Guid Id { get; }

        public string Name { get; }

        public string Namespace { get; }
    }
}
