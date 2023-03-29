// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.Common.iOS;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    /// <summary>
    ///     Based on <see cref="T:UIKit.UIApplicationDelegate"/>, used for integration WhiteLabel components.
    /// </summary>
    public abstract class AppDelegateBase : UIApplicationDelegate
    {
        private UIViewController _rootViewController = default!;

        /// <inheritdoc />
        public override UIWindow? Window { get; set; }

        /// <inheritdoc />
        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            // YP: Hard reference kept because StoryboardNavigation service uses weak references.
            _rootViewController = CreateRootViewController();

            InitializeMainWindow(_rootViewController);
            InitializeWhiteLabelRuntime();

            return true;
        }

        protected virtual UIViewController CreateRootViewController()
        {
            return new UINavigationController();
        }

        protected virtual void InitializeMainWindow(UIViewController rootViewController)
        {
            Window = new UIWindow(UIScreen.MainScreen.Bounds)
            {
                RootViewController = rootViewController
            };
            Window.MakeKeyAndVisible();
        }

        protected virtual void InitializeWhiteLabelRuntime()
        {
            // Init Bindings
            BindingExtensions.Initialize(new AppleBindingFactory());

            // Init platform helpers
            Execute.Initialize(new IosMainThreadExecutor());

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

        protected abstract void InitializeNavigation(IContainer container);
    }
}
