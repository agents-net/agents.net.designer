using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Agents.Net.Designer.Model
{
    public class MessageModel
    {
        public MessageModel(string name = "", string @namespace = ".Messages",
                            Guid id = default, bool buildIn = false)
        {
            Name = name;
            Namespace = @namespace;
            Id = id == default ? Guid.NewGuid() : id;
            BuildIn = buildIn;
        }
        
        public Guid Id { get; }

        public string Name { get; }

        public string Namespace { get; }

        public bool BuildIn { get; }
    }
}
