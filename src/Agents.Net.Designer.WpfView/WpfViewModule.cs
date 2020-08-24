using System;
using System.Collections.Generic;
using System.Text;
using Agents.Net.Designer.WpfView.Agents;
using Autofac;

namespace Agents.Net.Designer.WpfView
{
    public class WpfViewModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWindowDataContextProvider>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindowObserver>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<ViewModelChangeApplier>().As<Agent>().InstancePerLifetimeScope();
            builder.RegisterType<MainWindow>().AsSelf().InstancePerLifetimeScope();
        }
    }
}
