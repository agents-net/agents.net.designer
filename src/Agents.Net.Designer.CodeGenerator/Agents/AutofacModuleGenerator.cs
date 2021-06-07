using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.CodeGenerator.Templates.Messages;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Intercepts(typeof(FilesGenerated))]
    [Consumes(typeof(GeneratorSettingsDefined))]
    [Consumes(typeof(TemplatesLoaded))]
    public class AutofacModuleGenerator : InterceptorAgent
    {
        private readonly MessageCollector<GeneratorSettingsDefined, FilesGenerated, TemplatesLoaded> messageCollector;

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
            messageCollector.PushAndExecute(messageData, set =>
            {
                set.MarkAsConsumed(set.Message2);
                
                if (set.Message1.Settings.GenerateAutofacModule)
                {
                    string contents = GenerateModule(set, out string name);
                    File.WriteAllText(Path.Combine(set.Message1.Path, $"{name}.cs"), contents, Encoding.UTF8);
                }
            });

            return InterceptionAction.Continue;
        }

        private static string GenerateModule(MessageCollection<GeneratorSettingsDefined, FilesGenerated, TemplatesLoaded> set, out string name)
        {
            string template = set.Message3.Templates["AutofacModuleTemplate"];

            List<FileGenerationResult> agents = new();
            List<FileGenerationResult> files = new();
            foreach (FileGenerationResult generationResult in set.Message2.FileResults)
            {
                if (generationResult.FileType.HasFlag(FileType.Agent))
                {
                    agents.Add(generationResult);
                }

                files.Add(generationResult);
            }

            string rootNamespace = FindRootNamespace(set.Message1, files);

            name = GetName(rootNamespace);

            List<string> agentDefinitions = new();
            int templatePosition = template.IndexOf("$registeragents$", StringComparison.Ordinal);
            int previousLineBreak = template.LastIndexOf('\n', templatePosition);
            int tabSpace = templatePosition - previousLineBreak - 1;
            string tab = new(' ', tabSpace);
            foreach (FileGenerationResult generationResult in agents)
            {
                string fullName = $"{generationResult.Namespace}.{generationResult.Name}";
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

        private static string FindRootNamespace(GeneratorSettingsDefined settingsDefined, IEnumerable<FileGenerationResult> files)
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
