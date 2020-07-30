using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Agents.Net.Designer.Model
{
    public static class ModelExtensions
    {
        public static string FullName(this MessageModel message, CommunityModel communityModel)
        {
            return $"{message.Namespace.ExtendNamespace(communityModel)}.{message.Name}";
        }

        public static string ExtendNamespace(this string modelNamespace, CommunityModel communityModel)
        {
            if (string.IsNullOrEmpty(modelNamespace))
            {
                return communityModel.GeneratorSettings?.PackageNamespace ?? string.Empty;
            }
            
            if (modelNamespace.StartsWith('.'))
            {
                if (string.IsNullOrEmpty(communityModel.GeneratorSettings?.PackageNamespace))
                {
                    modelNamespace = modelNamespace.Substring(1);
                }
                else
                {
                    modelNamespace = communityModel.GeneratorSettings.PackageNamespace + modelNamespace;
                }
            }

            return modelNamespace;
        }

        public static string FullName(this AgentModel agent, CommunityModel communityModel)
        {
            return $"{agent.Namespace.ExtendNamespace(communityModel)}.{agent.Name}";
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
