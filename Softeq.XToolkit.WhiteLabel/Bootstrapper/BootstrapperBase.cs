// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Services;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    /// <summary>
    ///     Base class for registration/overriding dependencies.
    /// </summary>
    public abstract class BootstrapperBase : IBootstrapper
    {
        public IContainer Initialize()
        {
            var containerBuilder = CreateContainerBuilder();

            // framework level
            RegisterInternalServices(containerBuilder);
            RegisterFromAssemblies(containerBuilder);

            // application level
            ConfigureIoc(containerBuilder);

            return BuildContainer(containerBuilder);
        }

        /// <summary>
        ///     Creates IoC container builder. It will be used to build IoC container later.
        /// </summary>
        /// <returns>IoC container builder.</returns>
        protected virtual IContainerBuilder CreateContainerBuilder()
        {
            return new DryIocContainerBuilder();
        }

        /// <summary>
        ///     Registers internal services in IoC container builder.
        /// </summary>
        /// <param name="builder">IoC container builder.</param>
        protected virtual void RegisterInternalServices(IContainerBuilder builder)
        {
            // logs
            builder.Singleton<ConsoleLogManager, ILogManager>(IfRegistered.Keep);

            // navigation
            builder.Singleton<PageNavigationService, IPageNavigationService>(IfRegistered.Keep);
            builder.Singleton<BackStackManager, IBackStackManager>(IfRegistered.Keep);

            // json
            builder.Singleton<DefaultJsonSerializer, IJsonSerializer>();
        }

        /// <summary>
        ///     Registers additional types from the specified assemblies in the specified
        ///     IoC container builder.
        ///     Assemblies should be specified as the return value of the <see cref="SelectAssemblies"/> method.
        /// </summary>
        /// <param name="builder">IoC container builder.</param>
        protected virtual void RegisterFromAssemblies(IContainerBuilder builder)
        {
            var assemblies = SelectAssemblies();
            InitializeAssemblySource(assemblies);
            RegisterTypesFromAssemblies(builder, assemblies);
        }

        /// <summary>
        ///     Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>The list of assemblies to inspect.</returns>
        protected abstract IList<Assembly> SelectAssemblies();

        /// <summary>
        ///    Initializes <see cref="AssemblySourceCache"/>.
        /// </summary>
        /// <param name="assemblies">List of assemblies to cache.</param>
        protected virtual void InitializeAssemblySource(IEnumerable<Assembly> assemblies)
        {
            AssemblySourceCache.Install();

            AssemblySourceCache.ExtractTypes = assembly =>
            {
                var types = assembly.GetExportedTypes().Where(IsExtractToAssembliesCache);
                return types;
            };

            AssemblySource.Instance.ReplaceRange(assemblies);
        }

        /// <summary>
        ///     The predicate of extracting type for storing in the cache.
        /// </summary>
        /// <param name="type"><see cref="T:System.Type"/> of the object.</param>
        /// <returns><see langword="true"/> when the type should be extracted.</returns>
        protected abstract bool IsExtractToAssembliesCache(Type type);

        /// <summary>
        ///     Registers types from the specified assemblies
        ///     in the specified IoC container builder.
        /// </summary>
        /// <param name="builder">IoC container builder.</param>
        /// <param name="assemblies">List of source assemblies to register.</param>
        protected virtual void RegisterTypesFromAssemblies(IContainerBuilder builder, IList<Assembly> assemblies)
        {
        }

        /// <summary>
        ///     Override to configure the framework and setup your IoC container.
        /// </summary>
        /// <param name="builder">IoC container builder.</param>
        protected abstract void ConfigureIoc(IContainerBuilder builder);

        /// <summary>
        ///    Builds IoC container.
        /// </summary>
        /// <param name="builder">IoC container builder.</param>
        /// <returns>IoC container.</returns>
        protected virtual IContainer BuildContainer(IContainerBuilder builder)
        {
            return builder.Build();
        }
    }
}
