using System;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Templates.Agents
{
    public class TemplateFileLoader : Agent
    {
        #region Definition

        [AgentDefinition]
        public static AgentDefinition TemplateFileLoaderDefinition { get; }
            = new AgentDefinition(new []
                                  {
                                      TemplateFileFound.TemplateFileFoundDefinition
                                  },
                                  new []
                                  {
                                      TemplateLoaded.TemplateLoadedDefinition
                                  });

        #endregion

        public TemplateFileLoader(IMessageBoard messageBoard) : base(TemplateFileLoaderDefinition, messageBoard)
        {
        }

        protected override void ExecuteCore(Message messageData)
        {
            TemplateFileFound fileFound = messageData.Get<TemplateFileFound>();
            string name = Path.GetFileNameWithoutExtension(fileFound.Path);
            string content = File.ReadAllText(fileFound.Path);
            OnMessage(new TemplateLoaded(name, content, messageData));
        }
    }
}
