// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using AndroidX.Fragment.App;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation.FrameNavigation;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid
{
    public abstract class DroidBootstrapperBase : BootstrapperWithViewModelLookup
    {
        protected override IViewModelFinder ViewModelFinder { get; } =
            new DefaultViewModelFinder(
                typeof(FragmentActivity),
                typeof(DialogFragment),
                typeof(Fragment));

        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            base.RegisterInternalServices(builder);

            // common
            builder.Singleton<EssentialsContextProvider, IContextProvider>(IfRegistered.Keep);

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
}
