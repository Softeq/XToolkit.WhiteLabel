// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public abstract class BootstrapperBase : IBootstrapper
    {
        public void Init(IList<Assembly> assemblies)
        {
            var containerBuilder = CreateContainerBuilder();

            RegisterInternalServices(containerBuilder);
            ConfigureIoc(containerBuilder);

            var container = BuildContainer(containerBuilder, assemblies);

            Dependencies.Initialize(container);
        }

        protected virtual IContainerBuilder CreateContainerBuilder()
        {
            return new DryIocContainerBuilder();
        }

        protected virtual void RegisterInternalServices(IContainerBuilder builder)
        {
            // logs
            builder.Singleton<ConsoleLogManager, ILogManager>(IfRegistered.Keep);

            // navigation
            builder.Singleton<PageNavigationService, IPageNavigationService>(IfRegistered.Keep);
            builder.Singleton<BackStackManager, IBackStackManager>(IfRegistered.Keep);

            // tabs
            builder.Singleton<TabNavigationService, ITabNavigationService>(IfRegistered.Keep);
            builder.PerDependency<TabViewModel>(IfRegistered.Keep);
        }

        protected abstract void ConfigureIoc(IContainerBuilder builder);

        protected virtual IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            return builder.Build();
        }
    }
}
