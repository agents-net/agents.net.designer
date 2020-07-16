using System;
using System.Collections.Generic;
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
    }
}
