// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Controls;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using UIKit;

namespace Playground.iOS.ViewControllers.Controls
{
    public partial class PhotoBrowserViewController : ViewControllerBase<PhotoBrowserViewModel>
    {
        public PhotoBrowserViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            GalleryButton.SetCommand(ViewModel.GalleryCommand);
            CameraButton.SetCommand(ViewModel.CameraCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Image, OnImage);
        }

        private void OnImage(IDisposable? image)
        {
            ImageView.Image = (UIImage) image!;
        }
    }
}
