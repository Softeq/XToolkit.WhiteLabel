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

            ConfigureIoc(containerBuilder);
            RegisterInternalServices(containerBuilder);

            var container = BuildContainer(containerBuilder, assemblies);

            Dependencies.Initialize(container);
        }

        protected virtual IContainerBuilder CreateContainerBuilder()
        {
            return new DryIocContainerBuilder();
        }

        protected abstract void ConfigureIoc(IContainerBuilder builder);

        protected abstract void RegisterInternalServices(IContainerBuilder builder);

        protected virtual IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            // navigation
            builder.Singleton<PageNavigationService, IPageNavigationService>();
            builder.Singleton<BackStackManager, IBackStackManager>();

            // tabs
            builder.Singleton<TabNavigationService, ITabNavigationService>();
            builder.PerDependency<TabViewModel>();

            // logs
            builder.Singleton<ConsoleLogManager, ILogManager>();

            return builder.Build();
        }
    }
}
