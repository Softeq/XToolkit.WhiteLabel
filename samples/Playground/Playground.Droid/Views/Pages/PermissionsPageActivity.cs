using System;
using Android.App;
using Android.Graphics;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.Permissions;

namespace Playground.Droid.Views.Pages
{
    [Activity(Theme = "@style/AppTheme")]
    public class PermissionsPageActivity : ActivityBase<PermissionsPageViewModel>
    {
        private Button _cameraButton;
        private Button _storageButton;
        private Button _locationButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_permissions);

            _cameraButton = FindViewById<Button>(Resource.Id.buttonCamera);
            _cameraButton.SetCommand(ViewModel.RequestCalendarPermissionCommand, Permission.Camera);

            _storageButton = FindViewById<Button>(Resource.Id.buttonStorage);
            _storageButton.SetCommand(ViewModel.RequestCalendarPermissionCommand, Permission.Storage);

            _locationButton = FindViewById<Button>(Resource.Id.buttonLocationInUse);
            _locationButton.SetCommand(ViewModel.RequestCalendarPermissionCommand, Permission.LocationAlways);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();
            Bindings.Add(this.SetBinding(() => ViewModel.CameraGranted).WhenSourceChanges(() =>
            {
                _cameraButton.SetBackgroundColor(GetColor(ViewModel.CameraGranted));
            }));
            Bindings.Add(this.SetBinding(() => ViewModel.StorageGranted).WhenSourceChanges(() =>
            {
                _storageButton.SetBackgroundColor(GetColor(ViewModel.StorageGranted));
            }));
            Bindings.Add(this.SetBinding(() => ViewModel.LocationAlwaysGranted).WhenSourceChanges(() =>
            {
                _locationButton.SetBackgroundColor(GetColor(ViewModel.LocationAlwaysGranted));
            }));
        }

        private Color GetColor(bool granted)
        {
            return granted ? Color.Green : Color.Red;
        }
    }
}
