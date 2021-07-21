#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.Linq;

namespace Agents.Net.Designer.Model.Messages
{
    public class ModelModificationBatch : MessageDecorator
    {
        private ModelModificationBatch(Message decoratedMessage, ModifyModel[] modifications, int index, IEnumerable<Message> additionalPredecessors = null)
            : base(decoratedMessage, additionalPredecessors)
        {
            Modifications = modifications;
            Index = index;
        }
        
        public ModifyModel[] Modifications { get; }
        
        public int Index { get; }

        private bool IsCompleted => Index == Modifications.Length - 1;

        public static ModelModificationBatch Create(ModifyModel[] modifications)
        {
            ModifyModel first = modifications[0];
            ModelModificationBatch message = new(first, modifications, 0);
            return message;
        }

        public static bool TryContinue(ModelModificationBatch batch, IEnumerable<Message> predecessors, out ModelModificationBatch newBatch)
        {
            if (batch.IsCompleted)
            {
                newBatch = null;
                return false;
            }

            ModifyModel nextModification = batch.Modifications[batch.Index + 1];
            newBatch = new ModelModificationBatch(nextModification, batch.Modifications, batch.Index + 1,
                                                  predecessors);
            return true;
        }

        protected override string DataToString()
        {
            return
                $"{nameof(Modifications)}: {string.Join(", ", Modifications.Select(m => m.Id))}; {nameof(Index)}: {Index}";
        }
    }
}