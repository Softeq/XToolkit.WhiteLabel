// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    /// <summary>
    ///     The class that encapsulates logic about auto-registering ViewModels and re-configuring ViewLocator.
    /// </summary>
    public abstract class BootstrapperWithViewModelLookup : BootstrapperBase
    {
        protected abstract IViewModelFinder ViewModelFinder { get; }

        protected override void RegisterTypesFromAssemblies(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            base.RegisterTypesFromAssemblies(builder, assemblies);

            FindAndRegisterViewModels(builder, assemblies);
        }

        protected virtual void FindAndRegisterViewModels(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            var viewModelViewMap = ViewModelFinder.GetViewModelToViewMapping(assemblies);

            RegisterViewModels(builder, viewModelViewMap);

            builder.Singleton(_ => viewModelViewMap, IfRegistered.Keep);
        }

        protected virtual void RegisterViewModels(IContainerBuilder builder, ViewModelToViewMap viewModelToViewMap)
        {
            foreach (var (viewModelType, _) in viewModelToViewMap)
            {
                builder.PerDependency(viewModelType, IfRegistered.Keep);
            }
        }

        /// <inheritdoc />
        protected override bool IsExtractToAssembliesCache(Type type)
        {
            return ViewModelFinder.IsViewType(type);
        }
    }
}
