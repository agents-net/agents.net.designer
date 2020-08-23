using System;
using System.Collections.Generic;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public class MessageDecoratorModel : MessageModel
    {
        public MessageDecoratorModel(string name = "", string @namespace = ".Messages", Guid id = default,
                 bool buildIn = false, Guid decoratedMessage = default)
            : base(name, @namespace, id,
                   buildIn)
        {
            DecoratedMessage = decoratedMessage;
        }

        public override MessageModel Clone(string name = null, string @namespace = null, Guid? id = null,
                                           bool? buildIn = null)
        {
            return new MessageDecoratorModel(name ?? Name,
                                             @namespace ?? Namespace,
                                             id ?? Id,
                                             buildIn ?? BuildIn,
                                             DecoratedMessage);
        }

        public virtual MessageModel Clone(Guid decoratedMessage, string name = null, string @namespace = null, Guid? id = null,
                                           bool? buildIn = null)
        {
            return new MessageDecoratorModel(name ?? Name,
                                             @namespace ?? Namespace,
                                             id ?? Id,
                                             buildIn ?? BuildIn,
                                             decoratedMessage);
        }

        public Guid DecoratedMessage { get; }
    }
}
