// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    /// <summary>
    ///     Based on <see cref="T:UIKit.UIApplicationDelegate"/>, used for integration WhiteLabel components.
    /// </summary>
    public abstract class AppDelegateBase : UIApplicationDelegate
    {
        private UIViewController _rootNavigationController = default!;

        public override UIWindow Window { get; set; } = default!;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // YP: Hard reference kept because StoryboardNavigation service uses weak references.
            _rootNavigationController = CreateRootNavigationController();

            InitializeMainWindow();
            InitializeWhiteLabelRuntime();

            return true;
        }

        protected virtual UINavigationController CreateRootNavigationController()
        {
            return new UINavigationController();
        }

        protected virtual void InitializeMainWindow()
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                BackgroundColor = UIColor.SystemBackgroundColor,
                RootViewController = _rootNavigationController
            };
            Window.MakeKeyAndVisible();
        }

        protected virtual void InitializeWhiteLabelRuntime()
        {
            // Init Bindings
            BindingExtensions.Initialize(new AppleBindingFactory());

            // Init platform helpers
            PlatformProvider.Current = new IosPlatformProvider();

            // Init dependencies
            var bootstrapper = CreateBootstrapper();
            var container = bootstrapper.Initialize();
            Dependencies.Initialize(container);

            // Notify dependencies ready to be used
            OnContainerInitialized(container);
        }

        protected abstract IBootstrapper CreateBootstrapper();

        protected virtual void OnContainerInitialized(IContainer container)
        {
            InitializeNavigation(container);
        }

        protected virtual void InitializeNavigation(IContainer container)
        {
            var navigationService = container.Resolve<IPageNavigationService>();
            navigationService.Initialize(Window.RootViewController);
            ConfigureEntryPointNavigation(navigationService);
        }

        protected abstract void ConfigureEntryPointNavigation(IPageNavigationService navigationService);
    }
}
