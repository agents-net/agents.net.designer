using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Agents.Net.Designer.Generator.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    public static class Extensions
    {
        public static void GenerateFile(this GeneratingFile file, string template)
        {
            string content = template.Replace("$rootnamespace$", file.Namespace, StringComparison.Ordinal)
                                     .Replace("$itemname$", file.Name, StringComparison.Ordinal);
            File.WriteAllText(file.Path, content, Encoding.UTF8);
        }
    }
}
