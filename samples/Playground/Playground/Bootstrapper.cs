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
        public static void Configure(ContainerBuilder builder)
        {
            // common
            builder.Singleton<JsonSerializer, IJsonSerializer>();

            // navigation
            builder.Singleton<PageNavigationService, IPageNavigationService>();
            builder.Singleton<BackStackManager, IBackStackManager>();

            RegisterServiceLocator(builder);
        }

        private static void RegisterServiceLocator(ContainerBuilder builder)
        {
            var serviceLocator = new IocContainer();

            Dependencies.Initialize(serviceLocator);

            builder.Singleton<IIocContainer>(c => serviceLocator);
        }
    }
}
