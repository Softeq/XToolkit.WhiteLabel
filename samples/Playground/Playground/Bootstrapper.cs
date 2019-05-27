// Developed by Softeq Development Corporation
// http://www.softeq.com

using Autofac;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Playground
{
    public static class Bootstrapper
    {
        public static void Configure(ContainerBuilder containerBuilder)
        {
            // common
            containerBuilder.PerLifetimeScope<JsonSerializer, IJsonSerializer>();

            // navigation
            containerBuilder.PerLifetimeScope<PageNavigationService, IPageNavigationService>();
            containerBuilder.PerLifetimeScope<BackStackManager, IBackStackManager>();

            RegisterServiceLocator(containerBuilder);
        }

        private static void RegisterServiceLocator(ContainerBuilder containerBuilder)
        {
            var serviceLocator = new IocContainer();

            Dependencies.Initialize(serviceLocator);

            containerBuilder.PerLifetimeScope<IIocContainer>(c => serviceLocator);
        }
    }
}
