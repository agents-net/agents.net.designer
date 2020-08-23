using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public static class ModelExtensions
    {
        public static string FullName(this MessageModel message)
        {
            return $"{message.Namespace.ExtendNamespace(message)}.{message.Name}";
        }

        public static string FullNamespace(this MessageModel message)
        {
            return message.Namespace.ExtendNamespace(message);
        }

        public static string FullNamespace(this AgentModel agent)
        {
            return agent.Namespace.ExtendNamespace(agent);
        }

        public static string ExtendNamespace(this string modelNamespace, MessageModel messageModel)
        {
            return modelNamespace.ExtendNamespace(messageModel.ContainingPackage.GeneratorSettings);
        }

        public static string ExtendNamespace(this string modelNamespace, AgentModel agentModel)
        {
            return modelNamespace.ExtendNamespace(agentModel.ContainingPackage.GeneratorSettings);
        }

        private static string ExtendNamespace(this string modelNamespace, GeneratorSettings settings)
        {
            if (string.IsNullOrEmpty(modelNamespace))
            {
                return settings.PackageNamespace ?? string.Empty;
            }
            
            if (modelNamespace.StartsWith(".", StringComparison.Ordinal))
            {
                if (string.IsNullOrEmpty(settings.PackageNamespace))
                {
                    modelNamespace = modelNamespace.Substring(1);
                }
                else
                {
                    modelNamespace = settings.PackageNamespace + modelNamespace;
                }
            }

            return modelNamespace;
        }

        public static string FullName(this AgentModel agent)
        {
            return $"{agent.Namespace.ExtendNamespace(agent)}.{agent.Name}";
        }

        public static T AssertTypeOf<T>(this object value)
        {
            if (value == null)
            {
                return default;
            }
            if (value is T result)
            {
                return result;
            }
            throw new InvalidOperationException($"{value} was expected to be {typeof(T)}, but is {value?.GetType()}");
        }
    }
}
