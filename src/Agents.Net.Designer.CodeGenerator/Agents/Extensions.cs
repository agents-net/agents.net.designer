using System.IO;
using System.Text;
using Agents.Net.Designer.CodeGenerator.Messages;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    public static class Extensions
    {
        public static void GenerateFile(this GeneratingFile file, string template)
        {
            string content = template.Replace("$rootnamespace$", file.Namespace)
                                     .Replace("$itemname$", file.Name);
            File.WriteAllText(file.Path, content, Encoding.UTF8);
        }
    }
}
