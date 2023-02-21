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
    [Register(nameof(FramesYellowViewController))]
    public partial class FramesYellowViewController : ViewControllerBase<YellowViewModel>
    {
        public FramesYellowViewController(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            BackButton.SetCommand(ViewModel.BackCommand);
            BackButton.SetTitle(ViewModel.BackText, UIControlState.Normal);
        }
    }
}
