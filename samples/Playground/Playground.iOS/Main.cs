// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using UIKit;

namespace Playground.iOS
{
    public class Application
    {
        // This is the main entry point of the application.
        private static void Main(string[] args)
        {
            try
            {
                // if you want to use a different Application Delegate class from "AppDelegate"
                // you can specify it here.
                UIApplication.Main(args, null, nameof(AppDelegate));
            }
            catch (Exception exception)
            {
                Console.WriteLine($"CRASH!!! --> {exception}");
                throw;
            }
        }
    }
}