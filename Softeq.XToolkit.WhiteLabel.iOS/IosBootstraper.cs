// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public class IosBootstrapper : BootstrapperWithViewModelLookup
    {
        protected override ViewModelFinderBase ViewModelFinder { get; } = new IosViewModelFinder();

        protected override void RegisterInternalServices(IContainerBuilder builder)
        {
            base.RegisterInternalServices(builder);

            // navigation
            builder.Singleton<ViewControllerProvider, IViewControllerProvider>(IfRegistered.Keep);
            builder.Singleton<StoryboardViewLocator, IViewLocator>(IfRegistered.Keep);
            builder.Singleton<StoryboardNavigation, IPlatformNavigationService>(IfRegistered.Keep);
            builder.PerDependency<StoryboardFrameNavigationService, IFrameNavigationService>(IfRegistered.Keep);
        }

        protected override void ConfigureIoc(IContainerBuilder builder)
        {
        }
    }

    public class IosViewModelFinder : ViewModelFinderBase
    {
        protected override IEnumerable<Type> SelectViewsTypes(Assembly assembly)
        {
            return assembly.GetTypes().View(typeof(UIViewController));
        }
    }
}
