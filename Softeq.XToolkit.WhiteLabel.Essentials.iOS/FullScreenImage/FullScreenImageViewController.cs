// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using FFImageLoading;
using Softeq.XToolkit.WhiteLabel.Essentials.FullScreenImage;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.Essentials.iOS.FullScreenImage
{
    public partial class FullScreenImageViewController : ViewControllerBase<FullScreenImageViewModel>
    {
        public FullScreenImageViewController(IntPtr handle)
            : base(handle)
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
            var imageService = ImageService.Instance;

            var task = string.IsNullOrEmpty(ViewModel.ImagePath) == false
                ? imageService.LoadFile(ViewModel.ImagePath)
                : imageService.LoadUrl(ViewModel.ImageUrl);

            task.IntoAsync(ImageView);
        }
    }
}
