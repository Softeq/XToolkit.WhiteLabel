using Foundation;
using NetworkApp.iOS.CustomHandlers;
using Softeq.XToolkit.Remote.Client;
using UIKit;

namespace NetworkApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            HttpHandlerBuilder.NativeHandler = IgnoreSslClientHelper.CreateHandler();

            Xamarin.Forms.Forms.Init();
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}
