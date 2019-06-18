using Foundation;
using Playground.ViewModels.Pages;
using Softeq.XToolkit.WhiteLabel.iOS;
using System;
using UIKit;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Permissions;

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
            Bindings.Add(this.SetBinding(() => ViewModel.PhotosGranted).WhenSourceChanges(() =>
            {
                Photos.BackgroundColor = GetColor(ViewModel.PhotosGranted);
            }));
            Bindings.Add(this.SetBinding(() => ViewModel.CameraGranted).WhenSourceChanges(() =>
            {
                Camera.BackgroundColor = GetColor(ViewModel.CameraGranted);
            }));
            Bindings.Add(this.SetBinding(() => ViewModel.LocationInUseGranted).WhenSourceChanges(() =>
            {
                LocationInUse.BackgroundColor = GetColor(ViewModel.LocationInUseGranted);
            }));
            Bindings.Add(this.SetBinding(() => ViewModel.LocationAlwaysGranted).WhenSourceChanges(() =>
            {
                LocationAlways.BackgroundColor = GetColor(ViewModel.LocationAlwaysGranted);
            }));
        }

        private UIColor GetColor(bool granted)
        {
            return granted ? UIColor.Green : UIColor.Red;
        }
    }
}