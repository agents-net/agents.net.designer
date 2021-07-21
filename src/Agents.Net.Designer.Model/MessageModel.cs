#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class MessageModel
    {
        public MessageModel(string name = "", string @namespace = ".Messages",
                            Guid id = default, bool buildIn = false, bool isGeneric = false,
                            int genericParameterCount = 0, bool isGenericInstance = false)
        {
            Name = name;
            Namespace = @namespace;
            Id = id == default ? Guid.NewGuid() : id;
            BuildIn = buildIn;
            IsGeneric = isGeneric;
            GenericParameterCount = genericParameterCount;
            IsGenericInstance = isGenericInstance;
        }
        
        public virtual MessageModel Clone(string name = null, string @namespace = null,
                                          Guid? id = null, bool? buildIn = null,
                                          bool? isGeneric = null, int? genericParameterCount = null,
                                          bool? isGenericInstance = null)
        {
            return new(name ?? Name,
                       @namespace ?? Namespace,
                       id ?? Id,
                       buildIn ?? BuildIn,
                       isGeneric ?? IsGeneric,
                       genericParameterCount ?? GenericParameterCount,
                       isGenericInstance ?? IsGenericInstance);
        }

        public CommunityModel ContainingPackage { get; set; }
        
        public Guid Id { get; }

        public string Name { get; }

        public string Namespace { get; }

        public bool BuildIn { get; }

        public bool IsGeneric { get; }

        public int GenericParameterCount { get; }

        public bool IsGenericInstance { get; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(Name)}: {Name}, {nameof(Namespace)}: {Namespace}";
        }
    }
}
