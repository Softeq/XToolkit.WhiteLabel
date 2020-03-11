// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Reflection;
using Foundation;
using UIKit;
using Xamarin.Forms.Platform.iOS;

namespace Playground.Forms.iOS
{
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Xamarin.Forms.Forms.Init();

            LoadApplication(new App(
                new IosBootstrapper(),
                () => new List<Assembly>
                {
                    GetType().Assembly,
                    typeof(App).Assembly
                })
            );

            return base.FinishedLaunching(app, options);
        }
    }
}
