using System;
using System.IO;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    public class PathCompiler : InterceptorAgent
    {
        #region Definition

        [AgentDefinition]
        public static InterceptorAgentDefinition PathCompilerDefinition { get; }
            = new InterceptorAgentDefinition(new []
                                             {
                                                 ModelSelectedForGeneration.ModelSelectedForGenerationDefinition
                                             },
                                             Array.Empty<MessageDefinition>());

        #endregion

        public PathCompiler(IMessageBoard messageBoard) : base(PathCompilerDefinition, messageBoard)
        {
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            ModelSelectedForGeneration model = messageData.Get<ModelSelectedForGeneration>();
            string name;
            string[] namespaceParts;
            if (model.TryGet(out AgentModelSelectedForGeneration agentModel))
            {
                name = agentModel.Agent.Name;
                namespaceParts = agentModel.Agent.Namespace.Split(".", StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                MessageModelSelectedForGeneration messageModel = messageData.Get<MessageModelSelectedForGeneration>();
                name = messageModel.Message.Name;
                namespaceParts = messageModel.Message.Namespace.Split(".", StringSplitOptions.RemoveEmptyEntries);
            }

            string path = Path.Combine(model.GenerationPath, Path.Combine(namespaceParts), $"{name}.cs");
            //TODO replace with must lead to new on message and donotpublish
            model.ReplaceWith(new ModelSelectedForGeneration(path, model.Model, Array.Empty<Message>()));

            return InterceptionAction.Continue;
        }
    }
}
