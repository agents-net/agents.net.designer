using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Agents.Net;
using Agents.Net.Designer.Generator.Messages;
using Agents.Net.Designer.Templates.Messages;

namespace Agents.Net.Designer.Generator.Agents
{
    [Intercepts(typeof(FilesGenerated))]
    [Consumes(typeof(GeneratorSettingsDefined))]
    [Consumes(typeof(TemplatesLoaded))]
    [Consumes(typeof(GeneratingFile), Implicitly = true)]
    public class AutofacModuleGenerator : InterceptorAgent
    {        private readonly MessageCollector<GeneratorSettingsDefined, FilesGenerated, TemplatesLoaded> messageCollector;

        public AutofacModuleGenerator(IMessageBoard messageBoard) : base(messageBoard)
        {
            messageCollector = new MessageCollector<GeneratorSettingsDefined, FilesGenerated, TemplatesLoaded>();
        }

        protected override void ExecuteCore(Message messageData)
        {
            messageCollector.Push(messageData);
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            messageCollector.Push(messageData);
            MessageCollection<GeneratorSettingsDefined, FilesGenerated, TemplatesLoaded> set =
                messageCollector.FindSetsForDomain(messageData.MessageDomain).FirstOrDefault();
            if (set == null ||
                !set.Message1.Settings.GenerateAutofacModule)
            {
                return InterceptionAction.Continue;
            }

            string contents = GenerateModule(set, out string name);
            File.WriteAllText(Path.Combine(set.Message1.Path, $"{name}.cs"), contents, Encoding.UTF8);
            return InterceptionAction.Continue;
        }

        private static string GenerateModule(MessageCollection<GeneratorSettingsDefined, FilesGenerated, TemplatesLoaded> set, out string name)
        {
            string template = set.Message3.Templates["AutofacModuleTemplate"];

            List<GeneratingAgent> agents = new List<GeneratingAgent>();
            List<GeneratingFile> files = new List<GeneratingFile>();
            foreach (FileGenerated fileGenerated in set.Message2.PredecessorMessages)
            {
                if (fileGenerated.TryGetPredecessor(out GeneratingAgent agent))
                {
                    agents.Add(agent);
                }

                if (fileGenerated.TryGetPredecessor(out GeneratingFile file))
                {
                    files.Add(file);
                }
            }

            string rootNamespace = FindRootNamespace(set.Message1, files);

            name = GetName(rootNamespace);

            List<string> agentDefinitions = new List<string>();
            int templatePosition = template.IndexOf("$registeragents$", StringComparison.Ordinal);
            int previousLineBreak = template.LastIndexOf('\n', templatePosition);
            int tabSpace = templatePosition - previousLineBreak - 1;
            string tab = new string(' ', tabSpace);
            foreach (GeneratingAgent agent in agents)
            {
                GeneratingFile file = agent.Get<GeneratingFile>();
                string fullName = $"{file.Namespace}.{file.Name}";
                if (fullName.StartsWith(rootNamespace))
                {
                    fullName = fullName.Substring(rootNamespace.Length + 1);
                }
                agentDefinitions.Add($"builder.RegisterType<{fullName}>().As<Agent>().InstancePerLifetimeScope();");
            }

            string contents = set.Message3.Templates["AutofacModuleTemplate"]
                                 .Replace("$rootnamespace$", rootNamespace)
                                 .Replace("$itemname$", name)
                                 .Replace("$registeragents$",string.Join(Environment.NewLine+tab, agentDefinitions));
            return contents;
        }

        private static string GetName(string rootNamespace)
        {
            string name;
            name = rootNamespace;
            if (rootNamespace.Contains('.'))
            {
                name = rootNamespace.Substring(rootNamespace.LastIndexOf('.') + 1);
            }

            name = name + "Module";
            return name;
        }

        private static string FindRootNamespace(GeneratorSettingsDefined settingsDefined, List<GeneratingFile> files)
        {
            string rootNamespace = settingsDefined.Settings.PackageNamespace;
            if (string.IsNullOrEmpty(rootNamespace))
            {
                rootNamespace = string.Join(".", files.Select(f => f.Namespace)
                                                      .Select(s => s.Split(new[] {"."},
                                                                           StringSplitOptions.RemoveEmptyEntries)
                                                                    .AsEnumerable())
                                                      .Transpose()
                                                      .TakeWhile(s => s.All(x => x == s.First()))
                                                      .Select(s => s.First()));
                if (string.IsNullOrEmpty(rootNamespace))
                {
                    rootNamespace = Path.GetFileNameWithoutExtension(settingsDefined.Path);
                }
            }

            return rootNamespace;
        }
    }
}
