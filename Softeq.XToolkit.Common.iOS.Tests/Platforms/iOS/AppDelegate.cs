// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Microsoft.Maui;
using Microsoft.Maui.Hosting;

namespace Softeq.XToolkit.Common.iOS.Tests;

[Register(nameof(AppDelegate))]
public class AppDelegate : MauiUIApplicationDelegate
{
    protected override MauiApp CreateMauiApp() => MauiProgram.CreateMauiApp();
}
