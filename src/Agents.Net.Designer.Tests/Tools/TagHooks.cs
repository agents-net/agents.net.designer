using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;
using TechTalk.SpecFlow;

namespace Agents.Net.Designer.Tests.Tools
{
    [Binding]
    public sealed class TagHooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        private LoggingConfiguration emptyConfiguration;

        [BeforeScenario("enableLogging")]
        public void BeforeScenario()
        {
            emptyConfiguration = LogManager.Configuration;
            LoggingConfiguration config = new LoggingConfiguration();
            Layout layout = new JsonLayout
            {
                Attributes =
                {
                    new JsonAttribute("time", Layout.FromString(@"${longdate}")),
                    new JsonAttribute("level", Layout.FromString(@"${level:upperCase=true}")),
                    new JsonAttribute("message", Layout.FromString(@"${message}")),
                    new JsonAttribute("logger", Layout.FromString(@"${logger}")),
                    new JsonAttribute("exception", new JsonLayout
                    {
                        Attributes = 
                        { 
                            new JsonAttribute("type", "${exception:format=Type}"),
                            new JsonAttribute("message", "${exception:format=Message}"),
                            new JsonAttribute("stacktrace", "${exception:format=StackTrace}"),
                            new JsonAttribute("innerException", new JsonLayout
                            {
                                Attributes =
                                {
                                    new JsonAttribute("type", "${exception:format=:innerFormat=Type:MaxInnerExceptionLevel=5:InnerExceptionSeparator=}"),
                                    new JsonAttribute("message", "${exception:format=:innerFormat=Message:MaxInnerExceptionLevel=5:InnerExceptionSeparator=}"),
                                    new JsonAttribute("stacktrace", "${exception:format=:innerFormat=StackTrace:MaxInnerExceptionLevel=5:InnerExceptionSeparator=}"),
                                },
                                RenderEmptyObject = false
                            }, false)
                        },
                        RenderEmptyObject = false
                    },false),
                }
            };
            layout = new CompoundLayout
            {
                Layouts =
                {
                    layout,
                    Layout.FromString(",")
                }
            };
// Targets where to log to: File and Console
            string logFileName = ".\\log.txt";
            FileTarget logfile = new FileTarget("logfile")
            {
                FileName = logFileName,
                DeleteOldFileOnStartup = true,
                Layout = layout,
                Footer ="{}]",
                Header = "[",
            };
            BufferingTargetWrapper wrapper = new BufferingTargetWrapper(logfile, 1000, 100, BufferingTargetWrapperOverflowAction.Flush);
// Rules for mapping loggers to targets            
            config.AddRule(LogLevel.Trace, LogLevel.Fatal, wrapper);
            
// Apply config           
            LogManager.Configuration = config;
        }

        [AfterScenario("enableLogging")]
        public void AfterScenario()
        {
            LogManager.Configuration = emptyConfiguration;
        }
    }
}
