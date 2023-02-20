// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Components;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Converters;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Components
{
    public partial class PermissionsPageViewController : ViewControllerBase<PermissionsPageViewModel>
    {
        public PermissionsPageViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            Photos.SetCommand(ViewModel.Photos.RequestPermissionCommand);
            Camera.SetCommand(ViewModel.Camera.RequestPermissionCommand);
            LocationInUse.SetCommand(ViewModel.LocationInUse.RequestPermissionCommand);
            LocationAlways.SetCommand(ViewModel.LocationAlways.RequestPermissionCommand);
            Notifications.SetCommand(ViewModel.Notifications.RequestPermissionCommand); ;
            Bluetooth.SetCommand(ViewModel.Bluetooth.RequestPermissionCommand); ;
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            var converter = new ColorConverter();

            this.Bind(() => ViewModel.Photos.IsGranted, () => Photos.BackgroundColor, converter);
            this.Bind(() => ViewModel.Camera.IsGranted, () => Camera.BackgroundColor, converter);
            this.Bind(() => ViewModel.LocationInUse.IsGranted, () => LocationInUse.BackgroundColor, converter);
            this.Bind(() => ViewModel.LocationAlways.IsGranted, () => LocationAlways.BackgroundColor, converter);
            this.Bind(() => ViewModel.Notifications.IsGranted, () => Notifications.BackgroundColor, converter);
            this.Bind(() => ViewModel.Bluetooth.IsGranted, () => Bluetooth.BackgroundColor, converter);
        }

        private class ColorConverter : IConverter<UIColor?, bool>
        {
            public UIColor ConvertValue(bool value, object? parameter = null, string? language = null)
            {
                return value ? UIColor.Green : UIColor.Red;
            }

            public bool ConvertValueBack(UIColor? value, object? parameter = null, string? language = null)
            {
                return value!.Equals(UIColor.Green);
            }
        }
    }
}
