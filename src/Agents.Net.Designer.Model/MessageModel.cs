using System;
using System.Collections.Generic;
using System.Text;

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
        
        public virtual MessageModel Clone(string name = null, string @namespace = null,
                                          Guid? id = null, bool? buildIn = null)
        {
            return new MessageModel(name ?? Name,
                                    @namespace ?? Namespace,
                                    id ?? Id,
                                    buildIn ?? BuildIn);
        }

        public CommunityModel ContainingPackage { get; set; }
        
        public Guid Id { get; }

        public string Name { get; }

        public string Namespace { get; }

        public bool BuildIn { get; }
    }
}
