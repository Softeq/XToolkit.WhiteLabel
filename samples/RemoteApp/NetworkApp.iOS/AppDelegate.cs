// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using NetworkApp.iOS.CustomHandlers;
using UIKit;

namespace NetworkApp.iOS
{
    [Register("AppDelegate")]
    public partial class AppDelegate : Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            var customHttpMessageHandler = IgnoreSslClientHelper.CreateHandler();

            Xamarin.Forms.Forms.Init();
            LoadApplication(new App(customHttpMessageHandler));

            return base.FinishedLaunching(app, options);
        }
    }
}
