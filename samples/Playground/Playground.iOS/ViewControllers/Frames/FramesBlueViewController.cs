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
    [Register(nameof(FramesBlueViewController))]
    public partial class FramesBlueViewController : ViewControllerBase<BlueViewModel>
    {
        public FramesBlueViewController(IntPtr handle)
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
