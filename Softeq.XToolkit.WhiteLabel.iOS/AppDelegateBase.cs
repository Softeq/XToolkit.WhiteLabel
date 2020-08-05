// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.iOS;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS
{
    /// <summary>
    ///     Based on <see cref="T:UIKit.UIApplicationDelegate"/>, used for integration WhiteLabel components.
    /// </summary>
    public abstract class AppDelegateBase : UIApplicationDelegate
    {
        public override UIWindow Window { get; set; } = default!;

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            InitializeWhiteLabelRuntime();

            return true;
        }

        protected abstract IBootstrapper CreateBootstrapper();

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

        protected virtual void OnContainerInitialized(IContainer container)
        {
        }
    }
}
