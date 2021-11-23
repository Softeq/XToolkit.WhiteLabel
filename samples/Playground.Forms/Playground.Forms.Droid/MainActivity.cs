// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Softeq.XToolkit.Common.Droid.Permissions;
using Softeq.XToolkit.WhiteLabel;
using Xamarin.Forms.Platform.Android;

namespace Playground.Forms.Droid
{
    [Activity(
        Label = "Playground.Forms",
        Icon = "@mipmap/icon",
        Theme = "@style/MainTheme",
        MainLauncher = true,
        ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App(new DroidBootstrapper()));
        }

        // XToolkit.Permissions integration
        public override void OnRequestPermissionsResult(
            int requestCode,
            string[] permissions,
            [GeneratedEnum] Permission[] grantResults)
        {
            Dependencies.Container.Resolve<IPermissionRequestHandler>().Handle(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
