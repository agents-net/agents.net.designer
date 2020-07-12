using System;
using System.Collections.Generic;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Templates.Agents
{
    public class TemplatesFinder : Agent
    {
        private const string TemplatesLocation = ".\\Templates";

        #region Definition

        [AgentDefinition]
        public static AgentDefinition TemplatesFinderDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      InitializeMessage.InitializeMessageDefinition
                                  },
                                  new []
                                  {
                                      TemplateFileFound.TemplateFileFoundDefinition
                                  });

        #endregion

        public TemplatesFinder(IMessageBoard messageBoard) : base(TemplatesFinderDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            List<Message> messages = new List<Message>();
            foreach (string templateFile in Directory.EnumerateFiles(TemplatesLocation,"*.cs", SearchOption.AllDirectories))
            {
                messages.Add(new TemplateFileFound(templateFile, messageData));
            }
            OnMessages(messages);
        }
    }
}
