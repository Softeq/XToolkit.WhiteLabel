using System.Collections.Generic;
using System.Reflection;
using Autofac;
using Softeq.XToolkit.WhiteLabel.Extensions;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public abstract class BootstrapperBase : IBootstrapper
    {
        public void Init(IList<Assembly> assemblies)
        {
            var containerBuilder = new ContainerBuilder();
            ConfigureIoc(containerBuilder);
            RegisterServiceLocator(containerBuilder);
            RegisterInternalServices(containerBuilder);

            containerBuilder.RegisterBuildCallback(x => OnContainerReady());

            BuildContainer(containerBuilder, assemblies);
        }

        protected virtual void OnContainerReady()
        {
        }

        protected abstract void ConfigureIoc(ContainerBuilder builder);

        protected virtual void BuildContainer(ContainerBuilder builder, IList<Assembly> assemblies)
        {
            Dependencies.IocContainer.StartScope(builder);
        }

        protected abstract void RegisterInternalServices(ContainerBuilder builder);

        private void RegisterServiceLocator(ContainerBuilder builder)
        {
            if (!Dependencies.IsInitialized)
            {
                var serviceLocator = new IocContainer();
                Dependencies.Initialize(serviceLocator);
            }
            builder.Singleton<IIocContainer>(c => Dependencies.IocContainer)
                .PreserveExistingDefaults();
        }
    }
}
