// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using FFImageLoading.Work;
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

        private bool StatusBarHidden
        {
            set
            {
                var keyWindow = UIApplication.SharedApplication.KeyWindow;
                if (keyWindow != null)
                {
                    keyWindow.WindowLevel = value
                        ? UIWindowLevel.StatusBar
                        : UIWindowLevel.Normal;
                }
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            //TODO VPY: find better solution
            CloseButton.TintColor = ViewModel.FullScreenImageOptions.CloseButtonTintColor.UIColorFromHex();
            CloseButton.SetImage(UIImage.FromBundle(ViewModel.FullScreenImageOptions.IosCloseButtonImageBundleName), UIControlState.Normal);
            CloseButton.SetCommand(ViewModel.DialogComponent.CloseCommand, true);

            LoadImage();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            StatusBarHidden = true;
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            StatusBarHidden = false;
        }

        private void LoadImage()
        {
            TaskParameter task;

            if (string.IsNullOrEmpty(ViewModel.ImagePath) == false)
            {
                task = ImageService.Instance.LoadFile(ViewModel.ImagePath);
            }
            else
            {
                task = ImageService.Instance.LoadUrl(ViewModel.ImageUrl);
            }

            task.IntoAsync(ImageView);
        }
    }
}