#region Copyright
//  Copyright (c) Tobias Wilker and contributors
//  This file is licensed under MIT
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using Agents.Net.Designer.WpfView;
using Agents.Net.Designer.WpfView.Messages;
using Autofac;
using Serilog;
using Serilog.Core;
using Serilog.Formatting.Compact;

namespace Agents.Net.Designer
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    /// TODO Next tasks
    /// - Update agent framework after message domain visibility change
    /// - Split Graph Creator
    /// - Implement Assembly structure
    /// - extend scopes
    /// - Implement solution synchronization
    public partial class App : Application
    {
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
            ContainerBuilder builder = new();
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
                Log.Error(exception, $"Unhandled exception during setup.{Environment.NewLine}{exception}");
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
            if (File.Exists("log.json"))
            {
                File.Delete("log.json");
            }
            Log.Logger = new LoggerConfiguration()
                         .MinimumLevel.Verbose()
                         .WriteTo.Async(l => l.File(new CompactJsonFormatter(), "log.json"))
                         .CreateLogger();
        }
    }
}
