// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Net.Http;
using Foundation;
using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;

namespace Playground.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : AppDelegateBase
    {
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Environment.SetEnvironmentVariable("CFNETWORK_DIAGNOSTICS", "3", EnvironmentVariableTarget.Process);

            var result = base.FinishedLaunching(application, launchOptions);

            // Override point for customization after application launch.

            return result;
        }

        protected override IBootstrapper CreateBootstrapper()
        {
            return new CustomIosBootstrapper();
        }

        protected override void InitializeNavigation(IContainer container)
        {
            var navigationService = container.Resolve<IPageNavigationService>();
            navigationService.Initialize(Window!.RootViewController!);
            navigationService.For<StartPageViewModel>().Navigate();
        }
    }
}
