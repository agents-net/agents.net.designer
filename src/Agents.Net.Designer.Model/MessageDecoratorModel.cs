using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class MessageDecoratorModel : MessageModel
    {
        public MessageDecoratorModel(string name = "", string @namespace = ".Messages", Guid id = default,
                 bool buildIn = false, bool isGeneric = false, int genericParameterCount = 0,
                 bool isGenericInstance = false, Guid decoratedMessage = default)
            : base(name, @namespace, id,
                   buildIn, isGeneric, genericParameterCount,
                   isGenericInstance)
        {
            DecoratedMessage = decoratedMessage;
        }

        public override MessageModel Clone(string name = null, string @namespace = null,
                                           Guid? id = null, bool? buildIn = null,
                                           bool? isGeneric = null, int? genericParameterCount = null,
                                           bool? isGenericInstance = null)
        {
            return new MessageDecoratorModel(name ?? Name,
                                             @namespace ?? Namespace,
                                             id ?? Id,
                                             buildIn ?? BuildIn,
                                             isGeneric ?? IsGeneric,
                                             genericParameterCount ?? GenericParameterCount,
                                             isGenericInstance ?? IsGenericInstance,
                                             DecoratedMessage);
        }

        public virtual MessageModel Clone(Guid decoratedMessage, string @namespace = null, Guid? id = null,
                                           bool? buildIn = null, string name = null,
        bool? isGeneric = null, int? genericParameterCount = null,
        bool? isGenericInstance = null)
        {
            return new MessageDecoratorModel(name ?? Name,
                                             @namespace ?? Namespace,
                                             id ?? Id,
                                             buildIn ?? BuildIn,
                                             isGeneric ?? IsGeneric,
                                             genericParameterCount ?? GenericParameterCount,
                                             isGenericInstance ?? IsGenericInstance,
                                             decoratedMessage);
        }

        public Guid DecoratedMessage { get; }
    }
}
