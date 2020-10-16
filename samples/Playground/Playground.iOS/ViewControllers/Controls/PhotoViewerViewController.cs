// Developed by Softeq Development Corporation
// http://www.softeq.com

using Playground.ViewModels.Controls;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Controls
{
    public class PhotoViewerViewController : ViewControllerBase<PhotoViewerViewModel>
    {
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            InitView();
            InitButton();
        }

        private void InitView()
        {
            View.BackgroundColor = UIColor.White;
        }

        private void InitButton()
        {
            var openButton = new UIButton(UIButtonType.System);
            openButton.TranslatesAutoresizingMaskIntoConstraints = false;
            openButton.SetCommand(ViewModel.OpenCommand);
            openButton.SetTitle("Open FullScreen Image", UIControlState.Normal);

            View.AddSubview(openButton);

            NSLayoutConstraint.ActivateConstraints(new []
            {
                openButton.CenterXAnchor.ConstraintEqualTo(View.CenterXAnchor),
                openButton.CenterYAnchor.ConstraintEqualTo(View.CenterYAnchor),
            });
        }
    }
}
