#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Agents.Net.Designer.Model.Messages
{
    /// <summary>
    /// Message to extend the <see cref="ModificationRequest"/> modifications.
    /// </summary>
    /// <remarks>
    /// In order to register an extender this message needs to be intercepted and the extender registered.
    /// </remarks>
    public class ModificationRequestExtending : Message
    {
        public ModificationRequestExtending(Message predecessorMessage, IReadOnlyCollection<Modification> modifications)
            : base(predecessorMessage)
        {
            Modifications = modifications;
        }

        public ModificationRequestExtending(IEnumerable<Message> predecessorMessages, IReadOnlyCollection<Modification> modifications)
            : base(predecessorMessages)
        {
            Modifications = modifications;
        }

        public IReadOnlyCollection<Modification> Modifications { get; }

        private readonly ConcurrentBag<Func<List<Modification>, bool>>
            extender = new();

        public IReadOnlyCollection<Func<List<Modification>, bool>> Extender => extender.ToArray();

        public void RegisterExtender(Func<List<Modification>, bool> extenderFunc)
        {
            extender.Add(extenderFunc);
        }

        protected override string DataToString()
        {
            return string.Join(Environment.NewLine, Modifications);
        }
    }
}