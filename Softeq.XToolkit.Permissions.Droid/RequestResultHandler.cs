// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Content.PM;
using Android.Runtime;
using Softeq.XToolkit.Common.Droid.Permissions;

namespace Softeq.XToolkit.Permissions.Droid
{
    public class RequestResultHandler : IPermissionRequestHandler
    {
        public void Handle(int requestCode, string[] permissions, object grantResults)
        {
            HandleImpl(requestCode, permissions, (Permission[]) grantResults);
        }

        private void HandleImpl(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Microsoft.Maui.ApplicationModel.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
