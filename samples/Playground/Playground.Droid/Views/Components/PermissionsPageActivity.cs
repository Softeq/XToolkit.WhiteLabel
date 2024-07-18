// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.Components;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Converters;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Components
{
    [Activity]
    public class PermissionsPageActivity : ActivityBase<PermissionsPageViewModel>
    {
        private Button? _cameraButton;
        private Button? _storageButton;
        private Button? _locationInUseButton;
        private Button? _locationAlwaysButton;
        private Button? _notificationButton;
        private Button? _bluetoothButton;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_permissions);

            _cameraButton = FindViewById<Button>(Resource.Id.buttonCamera);
            _cameraButton!.SetCommand(ViewModel.Camera.RequestPermissionCommand);

            _storageButton = FindViewById<Button>(Resource.Id.buttonStorage);
            _storageButton!.SetCommand(ViewModel.Storage.RequestPermissionCommand);

            _locationInUseButton = FindViewById<Button>(Resource.Id.buttonLocationInUse);
            _locationInUseButton!.SetCommand(ViewModel.LocationInUse.RequestPermissionCommand);

            _locationAlwaysButton = FindViewById<Button>(Resource.Id.buttonLocationAlways);
            _locationAlwaysButton!.SetCommand(ViewModel.LocationAlways.RequestPermissionCommand);

            _notificationButton = FindViewById<Button>(Resource.Id.buttonNotification);
            _notificationButton!.SetCommand(ViewModel.Notifications.RequestPermissionCommand);

            _bluetoothButton = FindViewById<Button>(Resource.Id.buttonBluetooth);
            _bluetoothButton!.SetCommand(ViewModel.Bluetooth.RequestPermissionCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            var converter = new ColorConverter();

            this.Bind(() => ViewModel.Camera.IsGranted, () => _cameraButton!.Background, converter);
            this.Bind(() => ViewModel.Storage.IsGranted, () => _storageButton!.Background, converter);
            this.Bind(() => ViewModel.LocationInUse.IsGranted, () => _locationInUseButton!.Background, converter);
            this.Bind(() => ViewModel.LocationAlways.IsGranted, () => _locationAlwaysButton!.Background, converter);
            this.Bind(() => ViewModel.Notifications.IsGranted, () => _notificationButton!.Background, converter);
            this.Bind(() => ViewModel.Bluetooth.IsGranted, () => _bluetoothButton!.Background, converter);
        }

        private class ColorConverter : IConverter<Drawable?, bool>
        {
            public Drawable ConvertValue(bool @in, object? parameter = null, string? language = null)
            {
                return new ColorDrawable(@in ? Color.Green : Color.Red);
            }

            public bool ConvertValueBack(Drawable? value, object? parameter = null, string? language = null)
            {
                if (value is ColorDrawable colorDrawable)
                {
                    return colorDrawable.Color == Color.Green;
                }

                return false;
            }
        }
    }
}