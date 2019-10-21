// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public abstract class BootstrapperBase : IBootstrapper
    {
        public void Init(IList<Assembly> assemblies)
        {
            var containerBuilder = new DryIoCContainerBuilder();
            ConfigureIoc(containerBuilder);
            RegisterInternalServices(containerBuilder);

            Dependencies.Initialize(BuildContainer(containerBuilder, assemblies));
        }

        protected abstract void ConfigureIoc(IContainerBuilder builder);

        protected virtual IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            //navigation
            builder.Singleton<PageNavigationService, IPageNavigationService>();
            builder.Singleton<BackStackManager, IBackStackManager>();
            builder.Singleton<TabNavigationService, ITabNavigationService>(IfRegistered.Keep);
            builder.PerDependency<TabViewModel>(IfRegistered.Keep);

            //logs
            builder.Singleton<ConsoleLogManager, ILogManager>();

            return builder.Build();
        }

        protected abstract void RegisterInternalServices(IContainerBuilder builder);
    }
}
