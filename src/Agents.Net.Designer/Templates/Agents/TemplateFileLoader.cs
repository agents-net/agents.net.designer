using System;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Templates.Agents
{
    [Consumes(typeof(TemplateFileFound))]
    [Produces(typeof(TemplateLoaded))]
    public class TemplateFileLoader : Agent
    {        public TemplateFileLoader(IMessageBoard messageBoard) : base(messageBoard)
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
