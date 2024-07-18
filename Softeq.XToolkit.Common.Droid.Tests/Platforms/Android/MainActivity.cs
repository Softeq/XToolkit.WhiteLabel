// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Content.PM;
using Android.OS;
using Microsoft.Maui;

namespace Softeq.XToolkit.Common.Droid.Tests;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true,
    ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode |
                           ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    public static MainActivity Current { get; private set; } = null!;

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        Current = this;

        base.OnCreate(savedInstanceState);
    }
}
