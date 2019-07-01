// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.iOS;
using System;
using UIKit;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Permissions;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Common.Interfaces;

namespace Playground.iOS.ViewControllers.Pages
{
    public partial class PermissionsPageViewController : ViewControllerBase<PermissionsPageViewModel>
    {
        public PermissionsPageViewController (IntPtr handle) : base (handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Photos.SetCommand(ViewModel.RequestPhotosCommand);
            Camera.SetCommand(ViewModel.RequestCameraCommand);
            LocationInUse.SetCommand(ViewModel.RequestLocationInUseCommand);
            LocationAlways.SetCommand(ViewModel.RequestLocationAlwaysCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();
            var converter = new ColorConverter();

            this.Bind(() => ViewModel.PhotosGranted, () => Photos.BackgroundColor, converter);
            this.Bind(() => ViewModel.CameraGranted, () => Camera.BackgroundColor, converter);
            this.Bind(() => ViewModel.LocationInUseGranted, () => LocationInUse.BackgroundColor, converter);
            this.Bind(() => ViewModel.LocationAlwaysGranted, () => LocationAlways.BackgroundColor, converter);
        }

        private class ColorConverter : IConverter<UIColor, bool>
        {
            public UIColor ConvertValue(bool TIn, object parameter = null, string language = null)
            {
                return TIn ? UIColor.Green : UIColor.Red;
            }

            public bool ConvertValueBack(UIColor value, object parameter = null, string language = null)
            {
                return value == UIColor.Green;
            }
        }
    }
}
