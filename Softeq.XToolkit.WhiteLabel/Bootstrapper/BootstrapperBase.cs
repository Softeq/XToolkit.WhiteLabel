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

        protected virtual IContainerBuilder CreateContainerBuilder()
        {
            return new DryIocContainerBuilder();
        }

        /// <summary>
        ///     Registers internal services in DI container.
        /// </summary>
        /// <param name="builder">Implementation of DI container.</param>
        protected virtual void RegisterInternalServices(IContainerBuilder builder)
        {
            // logs
            builder.Singleton<ConsoleLogManager, ILogManager>(IfRegistered.Keep);

            // navigation
            builder.Singleton<PageNavigationService, IPageNavigationService>(IfRegistered.Keep);
            builder.Singleton<BackStackManager, IBackStackManager>(IfRegistered.Keep);

            // json
            builder.Singleton<Services.JsonSerializer, IJsonSerializer>();
        }

        /// <summary>
        ///     Registers additional types from the specified assemblies to DI container.
        ///     Assemblies should be specified as the return value of the <see cref="SelectAssemblies"/> method.
        /// </summary>
        /// <param name="builder">Implementation of DI container.</param>
        protected virtual void RegisterFromAssemblies(IContainerBuilder builder)
        {
            var assemblies = SelectAssemblies();
            InitializeAssemblySource(assemblies);
            RegisterTypesFromAssemblies(builder, assemblies);
        }

        /// <summary>
        ///     Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>A list of assemblies to inspect.</returns>
        protected abstract IList<Assembly> SelectAssemblies();

        /// <summary>
        ///    Initializes <see cref="AssemblySourceCache"/>.
        /// </summary>
        /// <param name="assemblies">List of Assemblies to cache.</param>
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
        /// <returns><c>true</c> when type should be extracted.</returns>
        protected abstract bool IsExtractToAssembliesCache(Type type);

        /// <summary>
        ///     Registers types from the specified assemblies.
        /// </summary>
        /// <param name="builder">Implementation of DI container.</param>
        /// <param name="assemblies">List of source Assemblies to register.</param>
        protected virtual void RegisterTypesFromAssemblies(IContainerBuilder builder, IList<Assembly> assemblies)
        {
        }

        /// <summary>
        ///     Override to configure the framework and setup your IoC container.
        /// </summary>
        /// <param name="builder">Implementation of DI container.</param>
        protected abstract void ConfigureIoc(IContainerBuilder builder);

        /// <summary>
        ///    Builds DI container.
        /// </summary>
        /// <param name="builder">Implementation of DI container.</param>
        /// <returns>DI container.</returns>
        protected virtual IContainer BuildContainer(IContainerBuilder builder)
        {
            return builder.Build();
        }
    }
}
