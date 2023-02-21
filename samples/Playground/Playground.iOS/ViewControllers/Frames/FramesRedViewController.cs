// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Frames
{
    [Register(nameof(FramesRedViewController))]
    public partial class FramesRedViewController : ViewControllerBase<RedViewModel>
    {
        public FramesRedViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            NextButton.SetCommand(ViewModel.NextCommand);
            NextButton.SetTitle(ViewModel.NextText, UIControlState.Normal);

            BackButton.SetCommand(ViewModel.BackCommand);
            BackButton.SetTitle(ViewModel.BackText, UIControlState.Normal);
        }
    }
}
