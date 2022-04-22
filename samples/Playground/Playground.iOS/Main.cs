// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Playground.iOS
{
#pragma warning disable SA1649
    public class Application
#pragma warning restore SA1649
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            // if you want to use a different Application Delegate class from "AppDelegate"
            // you can specify it here.
            UIApplication.Main(args, null, typeof(AppDelegate));
        }
    }
}
