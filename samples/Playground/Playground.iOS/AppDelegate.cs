// Developed by Softeq Development Corporation
// http://www.softeq.com

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
        private readonly UINavigationController _rootNavigationController = new UINavigationController();

        public override UIWindow Window { get; set; } = default!;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            var _ = base.FinishedLaunching(application, launchOptions);

            Window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = _rootNavigationController
            };
            Window.MakeKeyAndVisible();

            InitNavigation();

            return true;
        }

        protected override IBootstrapper Bootstrapper => new CustomIosBootstrapper();

        private void InitNavigation()
        {
            var navigationService = Dependencies.PageNavigationService;
            navigationService.Initialize(Window.RootViewController);

            // Entry point
            navigationService.For<StartPageViewModel>().Navigate();
        }
    }
}

