// Developed by Softeq Development Corporation
// http://www.softeq.com

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
    }
}
