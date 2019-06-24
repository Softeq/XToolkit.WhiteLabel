// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;
using Foundation;
using UIKit;
using Softeq.XToolkit.WhiteLabel;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Playground.ViewModels.Pages;

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

        protected override IList<Assembly> SelectAssemblies()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly =>
                    new[]
                    {
                        "Playground.iOS",
                        "Softeq.XToolkit.Chat.iOS",
                        "Softeq.XToolkit.WhiteLabel.iOS"
                    }
                    .Any(x => x.Equals(assembly.GetName().Name)))
                .ToList();
        }

        private void InitNavigation()
        {
            var navigationService = Dependencies.Container.Resolve<IPageNavigationService>();
            navigationService.Initialize(Window.RootViewController);

            // Entry point
            navigationService.For<StartPageViewModel>().Navigate();
        }
    }
}

