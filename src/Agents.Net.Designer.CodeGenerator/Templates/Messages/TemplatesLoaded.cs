#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;

namespace Agents.Net.Designer.CodeGenerator.Templates.Messages
{
    public class TemplatesLoaded : Message
    {
        public TemplatesLoaded(Dictionary<string, string> templates, Message predecessorMessage)
            : base(predecessorMessage)
        {
            Templates = templates;
        }

        public TemplatesLoaded(Dictionary<string, string> templates, IEnumerable<Message> predecessorMessages)
            : base(predecessorMessages)
        {
            Templates = templates;
        }

        public Dictionary<string, string> Templates { get; }

        protected override string DataToString()
        {
            return $"{nameof(Templates)}: {string.Join(", ",Templates.Keys)}";
        }
    }
}
