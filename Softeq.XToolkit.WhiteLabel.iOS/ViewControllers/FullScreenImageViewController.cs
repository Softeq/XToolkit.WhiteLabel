using System;
using FFImageLoading;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common.iOS.Extensions;
using Softeq.XToolkit.WhiteLabel.ViewModels;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.ViewControllers
{
    public partial class FullScreenImageViewController : ViewControllerBase<FullScreenImageViewModel>
    {
        public FullScreenImageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //TODO VPY: find better solution
            CloseButton.TintColor = ViewModel.FullScreenImageOptions.CloseButtonTintColor.UIColorFromHex();
            CloseButton.SetImage(UIImage.FromBundle(ViewModel.FullScreenImageOptions.IosCloseButtonImageBoundleName), UIControlState.Normal);
            CloseButton.SetCommand(ViewModel.DialogComponent.CloseCommand, true);

            ImageService.Instance.LoadUrl(ViewModel.ImageUrl).IntoAsync(ImageView);
        }
    }
}