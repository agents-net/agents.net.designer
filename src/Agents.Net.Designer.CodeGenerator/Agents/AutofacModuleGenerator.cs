#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.CodeGenerator.Templates.Messages;
using Agents.Net.Designer.FileSystem.Messages;
using Serilog;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    [Intercepts(typeof(FilesGenerated))]
    [Consumes(typeof(GeneratorSettingsDefined))]
    [Consumes(typeof(TemplatesLoaded))]
    [Consumes(typeof(FileOpened))]
    [Consumes(typeof(FileSystemException))]
    [Produces(typeof(FileOpening))]
    public class AutofacModuleGenerator : InterceptorAgent
    {
        private readonly MessageCollector<GeneratorSettingsDefined, FilesGenerated, TemplatesLoaded> messageCollector;
        private readonly MessageGate<FileOpening, FileOpened> fileGate = new();

        public AutofacModuleGenerator(IMessageBoard messageBoard) : base(messageBoard)
        {
            messageCollector = new MessageCollector<GeneratorSettingsDefined, FilesGenerated, TemplatesLoaded>();
        }

        protected override void ExecuteCore(Message messageData)
        {
            if (!messageCollector.TryPush(messageData))
            {
                fileGate.Check(messageData);
            }
        }

        protected override InterceptionAction InterceptCore(Message messageData)
        {
            InterceptionAction delay = InterceptionAction.Delay(out InterceptionDelayToken releaseToken);
            messageCollector.PushAndContinue(messageData, set =>
            {
                set.MarkAsConsumed(set.Message2);

                if (!set.Message1.Settings.GenerateAutofacModule)
                {
                    return;
                }

                string contents = GenerateModule(set, out string name);
                string path = Path.Combine(set.Message1.Path, $"{name}.cs");
                Log.Verbose($"Wait for continue with {name} opening");
                fileGate.SendAndContinue(new FileOpening(path, set), OnMessage,
                                         result =>
                                         {
                                             Log.Verbose($"File {name} opened");
                                             if (result.Result != MessageGateResultKind.Success)
                                             {
                                                 releaseToken.Release(DelayTokenReleaseIntention.DoNotPublish);
                                                 return;
                                             }

                                             using StreamWriter writer = new(result.EndMessage.Data, Encoding.UTF8);
                                             writer.Write(contents);
                                             releaseToken.Release();
                                         });
            });

            return delay;
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
