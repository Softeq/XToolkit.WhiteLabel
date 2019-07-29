// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Foundation;
using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : AppDelegateBase
    {
        private UINavigationController _rootNavigationController;

        public override UIWindow Window { get; set; }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var _ = base.FinishedLaunching(application, launchOptions);

            _rootNavigationController = new UINavigationController();

            Window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = _rootNavigationController
            };
            Window.MakeKeyAndVisible();

            InitNavigation();

            return true;
        }

        protected override IBootstrapper Bootstrapper => new CustomIosBootstrapper();

        protected override IList<Assembly> SelectAssemblies() => new List<Assembly>
        {
            GetType().Assembly,              // Playground.iOS
            typeof(AppDelegateBase).Assembly // Softeq.XToolkit.WhiteLabel.iOS
        };

        private void InitNavigation()
        {
            var navigationService = Dependencies.PageNavigationService;
            navigationService.Initialize(Window.RootViewController);

            // Entry point
            navigationService.For<StartPageViewModel>().Navigate();
        }
    }
}

