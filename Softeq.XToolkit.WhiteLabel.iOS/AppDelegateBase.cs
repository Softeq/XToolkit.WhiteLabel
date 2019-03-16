// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DryIoc;
using Foundation;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.WhiteLabel.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.iOS.Services;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    public abstract class AppDelegateBase : UIApplicationDelegate, IAppDelegate
    {
        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            //init factory for bindings
            BindingExtensions.Initialize(new AppleBindingFactory());

            //init assembly sources for Activator.cs
            AssemblySourceCache.Install();
            AssemblySourceCache.ExtractTypes = assembly =>
                assembly.GetExportedTypes()
                    .Where(t => typeof(UIViewController).IsAssignableFrom(t));
            AssemblySource.Instance.AddRange(SelectAssemblies());

            //init dependencies
            StartScopeForIoc();

            //init ui thread helper
            PlatformProvider.Current = new IosPlatformProvider();

            return true;
        }

        public ViewControllerBase GetRootViewFinder(UIViewController controller)
        {
            if (controller.PresentedViewController != null)
            {
                var presentedViewController = controller.PresentedViewController;
                return GetRootViewFinder(presentedViewController);
            }

            switch (controller)
            {
                case UINavigationController navigationController:
                    return GetRootViewFinder(navigationController.VisibleViewController);
                case UITabBarController tabBarController:
                    return GetRootViewFinder(tabBarController.SelectedViewController);
            }

            return (ViewControllerBase)controller;
        }

        protected abstract void ConfigureIoc(Container builder);

        protected abstract IList<Assembly> SelectAssemblies();

        protected virtual void StartScopeForIoc()
        {
            var containerBuilder = new Container();
            ConfigureIoc(containerBuilder);
            RegisterInternalServices(containerBuilder);

            Dependencies.IocContainer.StartScope(containerBuilder);
        }

        protected void RegisterInternalServices(Container builder)
        {
            builder.RegisterInstance<IAppDelegate>(this, IfAlreadyRegistered.Keep);
            builder.TryPerLifeTimeScope<StoryboardViewLocator, IViewLocator>();
            builder.TryPerLifeTimeScope<StoryboardNavigation, IPlatformNavigationService>();
            builder.TryPerDependency<StoryboardFrameNavigationService, IFrameNavigationService>();
        }
    }
}