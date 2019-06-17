// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public abstract class BootstrapperBase : IBootstrapper
    {
        public void Init(IList<Assembly> assemblies)
        {
            var containerBuilder = new AutofacContainerBuilder();
            ConfigureIoc(containerBuilder);
            RegisterInternalServices(containerBuilder);

            Dependencies.Initialize(BuildContainer(containerBuilder, assemblies));
        }

        protected abstract void ConfigureIoc(IContainerBuilder builder);

        protected virtual IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            return builder.Build();
        }

        protected abstract void RegisterInternalServices(IContainerBuilder builder);
    }
}
