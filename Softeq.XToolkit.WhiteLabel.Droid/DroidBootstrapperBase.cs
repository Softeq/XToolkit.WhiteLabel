// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Support.V4.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public abstract class DroidBootstrapperBase : BootstrapperWithViewModelLookup
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
            builder.Singleton<DroidViewLocator, IViewLocator>(IfRegistered.Keep);
            builder.PerDependency<DroidFrameNavigationService, IFrameNavigationService>(IfRegistered.Keep);
        }

        /// <inheritdoc />
        protected override IList<Assembly> SelectAssemblies()
        {
            return new List<Assembly>
            {
                typeof(DroidBootstrapperBase).Assembly
            };
        }
    }

    public class DroidViewModelFinder : ViewModelFinderBase
    {
        public override bool IsViewType(Type type)
        {
            return typeof(FragmentActivity).IsAssignableFrom(type)
                || typeof(DialogFragment).IsAssignableFrom(type)
                || typeof(Fragment).IsAssignableFrom(type);
        }

        protected override IEnumerable<Type> SelectViewsTypes(Assembly assembly)
        {
            return assembly.GetTypes().View(
                typeof(FragmentActivity),
                typeof(DialogFragment),
                typeof(Fragment));
        }
    }
}
