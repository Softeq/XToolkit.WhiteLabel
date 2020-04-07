// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Playground.Forms.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();

            LoadApplication(new App(new IosBootstrapper()));

            return base.FinishedLaunching(app, options);
        }
    }
}
