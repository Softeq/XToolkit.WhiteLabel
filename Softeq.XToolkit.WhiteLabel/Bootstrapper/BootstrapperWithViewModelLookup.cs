// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;

namespace Softeq.XToolkit.WhiteLabel.Bootstrapper
{
    public abstract class BootstrapperWithViewModelLookup : BootstrapperBase
    {
        protected abstract ViewModelFinderBase ViewModelFinder { get; }

        protected override IContainer BuildContainer(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            FindAndRegisterViewModels(builder, assemblies);

            return base.BuildContainer(builder, assemblies);
        }

        protected virtual void FindAndRegisterViewModels(IContainerBuilder builder, IList<Assembly> assemblies)
        {
            var viewModelToViewMapping = ViewModelFinder.GetViewModelToViewMapping(assemblies);

            RegisterViewModels(builder, viewModelToViewMapping);
            RegisterViewLocator(builder, viewModelToViewMapping);
        }

        protected virtual void RegisterViewModels(IContainerBuilder builder, Dictionary<Type, Type> viewModelToViewMapping)
        {
            foreach (var (viewModelType, _) in viewModelToViewMapping)
            {
                builder.PerDependency(viewModelType, IfRegistered.Keep);
            }
        }

        protected abstract void RegisterViewLocator(IContainerBuilder builder, Dictionary<Type, Type> viewModelToViewMapping);
    }
}
