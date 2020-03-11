// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Containers;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public abstract class BootstrapperBase : IBootstrapper
    {
        private bool _isInitialized;

        public void Initialize()
        {
            if (_isInitialized)
            {
                return;
            }

            _isInitialized = true;

            var containerBuilder = CreateContainerBuilder();

            RegisterInternalServices(containerBuilder);

            // assemblies
            var assemblies = SelectAssemblies();
            InitializeAssemblySource(assemblies);
            RegisterTypesFromAssemblies(containerBuilder, assemblies);

            // application level
            ConfigureIoc(containerBuilder);

            var container = BuildContainer(containerBuilder);

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
        }

        /// <summary>
        ///     Override to tell the framework where to find assemblies to inspect for views, etc.
        /// </summary>
        /// <returns>A list of assemblies to inspect.</returns>
        protected abstract IList<Assembly> SelectAssemblies();

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
        ///     The predicate of extracting type for storing in the cache
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        protected abstract bool IsExtractToAssembliesCache(Type type);

        protected virtual void RegisterTypesFromAssemblies(IContainerBuilder builder, IList<Assembly> assemblies)
        {
        }

        /// <summary>
        ///     Override to configure the framework and setup your IoC container
        /// </summary>
        /// <param name="builder"></param>
        protected abstract void ConfigureIoc(IContainerBuilder builder);

        protected virtual IContainer BuildContainer(IContainerBuilder builder)
        {
            return builder.Build();
        }
    }
}
