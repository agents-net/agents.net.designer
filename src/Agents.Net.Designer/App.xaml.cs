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
        private IContainer container;

        protected override void OnExit(ExitEventArgs e)
        {
            container?.Dispose();
            base.OnExit(e);
        }

        private void OnStartup(object sender, StartupEventArgs e)
        {
            IMessageBoard messageBoard;

            ConfigureLogging();

            //Create container
            ContainerBuilder builder = new ContainerBuilder();
            builder.RegisterModule(new DesignerModule());
            container = builder.Build();

            //Start agent community
            try
            {
                messageBoard = container.Resolve<IMessageBoard>();
                Agent[] agents = container.Resolve<IEnumerable<Agent>>().ToArray();
                messageBoard.Register(agents);
                messageBoard.Start();
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
