﻿#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System.Collections.Generic;
using System.IO;
using Agents.Net.Designer.CodeGenerator.Templates.Messages;

namespace Agents.Net.Designer.CodeGenerator.Templates.Agents
{
    [Consumes(typeof(InitializeMessage))]
    [Produces(typeof(TemplateFileFound))]
    public class TemplatesFinder : Agent
    {
        private const string TemplatesLocation = ".\\Templates";
        public TemplatesFinder(IMessageBoard messageBoard) : base(messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            List<Message> messages = new();
            foreach (string templateFile in Directory.EnumerateFiles(TemplatesLocation,"*.cs", SearchOption.AllDirectories))
            {
                messages.Add(new TemplateFileFound(templateFile, messageData));
            }
            OnMessages(messages);
        }
    }
}
