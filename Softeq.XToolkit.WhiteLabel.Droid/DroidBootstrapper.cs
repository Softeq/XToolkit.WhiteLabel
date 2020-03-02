// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public class DroidBootstrapper : BootstrapperWithViewModelLookup
    {
        protected override ViewModelFinderBase ViewModelFinder { get; } = new DroidViewModelFinder();

        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            base.RegisterInternalServices(builder);

            // common
            builder.Singleton(c => CrossCurrentActivity.Current, IfRegistered.Keep);

            // navigation
            builder.Singleton<ActivityPageNavigationService, IPlatformNavigationService>(IfRegistered.Keep);
            builder.Singleton<BundleService, IBundleService>(IfRegistered.Keep);
            builder.Singleton<DroidViewLocator>(IfRegistered.Keep);
            builder.PerDependency<DroidFrameNavigationService, IFrameNavigationService>(IfRegistered.Keep);
        }

        protected override void RegisterViewLocator(IContainerBuilder builder, Dictionary<Type, Type> viewModelToViewMapping)
        {
            builder.Singleton<IViewLocator>(container =>
            {
                var viewLocator = container.Resolve<DroidViewLocator>();
                viewLocator.Initialize(viewModelToViewMapping);
                return viewLocator;
            }, IfRegistered.Keep);
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
        }
    }

    public class DroidViewModelFinder : ViewModelFinderBase
    {
        protected override IEnumerable<Type> SelectViewModelTypes(Assembly assembly)
        {
            return assembly.GetTypes().View(typeof(AppCompatActivity), typeof(Fragment), typeof(DialogFragment));
        }
    }
}
