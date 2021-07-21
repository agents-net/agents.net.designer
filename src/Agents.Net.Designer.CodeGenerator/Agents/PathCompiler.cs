#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.IO;
using Agents.Net.Designer.CodeGenerator.Messages;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Intercepts(typeof(ModelSelectedForGeneration))]
    [Consumes(typeof(AgentModelSelectedForGeneration), Implicitly = true)]
    [Consumes(typeof(MessageModelSelectedForGeneration), Implicitly = true)]
    [Produces(typeof(ModelSelectedForGeneration))]
    public class PathCompiler : InterceptorAgent
    {
        public PathCompiler(IMessageBoard messageBoard) : base(messageBoard)
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
                namespaceParts = agentModel.Agent.Namespace.Split(new []{'.'}, StringSplitOptions.RemoveEmptyEntries);
            }
            else
            {
                MessageModelSelectedForGeneration messageModel = messageData.Get<MessageModelSelectedForGeneration>();
                name = messageModel.Message.Name;
                namespaceParts = messageModel.Message.Namespace.Split(new []{'.'}, StringSplitOptions.RemoveEmptyEntries);
            }

            string path = Path.Combine(model.GenerationPath, Path.Combine(namespaceParts), $"{name}.cs");
            //TODO replace with must lead to new on message and donotpublish
            model.ReplaceWith(new ModelSelectedForGeneration(path, Array.Empty<Message>()));

            return InterceptionAction.Continue;
        }
    }
}
