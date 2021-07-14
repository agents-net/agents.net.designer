using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Agents.Net.Designer.CodeGenerator.Messages;
using Agents.Net.Designer.FileSystem.Messages;
using Serilog;

namespace Agents.Net.Designer.CodeGenerator.Agents
{
    public static class Extensions
    {
        public static void GenerateFile(this GeneratingFile file, string template,
                                        MessageGate<FileOpening, FileOpened> fileGate,
                                        Action<Message> onMessage, IEnumerable<Message> predecessors,
                                        Action finalizeAction)
        {
            string content = template.Replace("$rootnamespace$", file.Namespace)
                                     .Replace("$itemname$", file.Name);
            
            Log.Verbose($"Wait for continue with {file.Name} opening");
            fileGate.SendAndContinue(new FileOpening(file.Path, predecessors), onMessage, result =>
            {
                Log.Verbose($"File {file.Name} opened");
                if (result.Result != MessageGateResultKind.Success)
                {
                    return;
                }

                using StreamWriter writer = new(result.EndMessage.Data, Encoding.UTF8);
                writer.Write(content);
                finalizeAction();
            });
        }
    }
}
