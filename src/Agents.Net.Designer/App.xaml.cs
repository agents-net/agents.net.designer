using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows;
using Agents.Net.Designer.View;
using Agents.Net.Designer.View.Messages;
using Autofac;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace Agents.Net.Designer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        private void OnStartup(object sender, StartupEventArgs e)
        {
            IMessageBoard messageBoard;

            ConfigureLogging();

            //Create container
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new DesignerModule());
            using IContainer container = builder.Build();

            //Start agent community
            try
            {
                messageBoard = container.Resolve<IMessageBoard>();
                Community community = container.Resolve<Community>();
                Agent[] agents = container.Resolve<IEnumerable<Agent>>().ToArray();
                community.RegisterAgents(agents);
                messageBoard.Start();
                AnalyseSystem(agents);
            }
            catch (Exception exception)
            {
                Logger.Log(LogLevel.Error, $"Unhandled exception during setup.{Environment.NewLine}{exception}");
                return;
            }

            //Show main window
            MainWindow mainWindow = container.Resolve<MainWindow>();
            mainWindow.Show();

            //Declare MainWindow as Message
            messageBoard.Publish(new MainWindowCreated(mainWindow));
        }

        private static void AnalyseSystem(Agent[] agents = null)
        {
            AnalysisResult analysisResult = CommunityAnalysis.Analyse(new[]
            {
                Assembly.GetAssembly(typeof(Agent)),
                Assembly.GetAssembly(typeof(App)),
            }, agents);

            Logger.Log(LogLevel.Info, $"Community Analysis - including {(agents == null ? $"{analysisResult.AnalysedAgentCount}" : $"{agents.Length}/{analysisResult.AnalysedAgentCount}")} " +
                              $"agents and {analysisResult.AnalysedMessageCount} messages.");
            Logger.Log(LogLevel.Info, "=====================================================");
            Logger.Log(LogLevel.Info, "Agents without definition:");
            Logger.Log(LogLevel.Info, string.Join(Environment.NewLine, analysisResult.AgentWithoutDefinition));
            Logger.Log(LogLevel.Info, "Message without definition:");
            Logger.Log(LogLevel.Info, string.Join(Environment.NewLine, analysisResult.MessageTypeWithoutDefinition));
            Logger.Log(LogLevel.Info, "Unused message definitions:");
            Logger.Log(LogLevel.Info, string.Join(Environment.NewLine, analysisResult.UnusedMessageDefinitions));
            Logger.Log(LogLevel.Info, "Unproduced message definitions:");
            Logger.Log(LogLevel.Info, string.Join(Environment.NewLine, analysisResult.UnproducedMessageTrigger));
            Logger.Log(LogLevel.Info, "Unconsumed message definitions:");
            Logger.Log(LogLevel.Info, string.Join(Environment.NewLine, analysisResult.UnconsumedMessageTrigger));
            if (agents != null)
            {
                
                Logger.Log(LogLevel.Info, "Not initialized agents:");
                Logger.Log(LogLevel.Info, string.Join(Environment.NewLine, analysisResult.NotInitializedAgents));
            }
        }

        private void ConfigureLogging()
        {
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
    }
}
