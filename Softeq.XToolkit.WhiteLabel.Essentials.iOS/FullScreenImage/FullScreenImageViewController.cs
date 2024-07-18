// Developed by Softeq Development Corporation
// http://www.softeq.com

using ObjCRuntime;
using Softeq.XToolkit.Bindings.iOS.Gestures;
using Softeq.XToolkit.WhiteLabel.Essentials.FullScreenImage;
using Softeq.XToolkit.WhiteLabel.iOS;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.Essentials.iOS.FullScreenImage
{
    public partial class FullScreenImageViewController : ViewControllerBase<FullScreenImageViewModel>
    {
        public FullScreenImageViewController(NativeHandle handle)
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

            InitView();

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

        private void InitView()
        {
            View!.Swipe(UISwipeGestureRecognizerDirection.Down).SetCommand(ViewModel.DialogComponent.CloseCommand);
        }

        private void LoadImage()
        {
            var imageService = Dependencies.Container.Resolve<IIosImageService>();

            var url = string.IsNullOrEmpty(ViewModel.ImagePath)
                ? ViewModel.ImageUrl
                : ViewModel.ImagePath;

            imageService.LoadImage(url!, ImageView);
        }
    }
}
