using System;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using UIKit;

namespace Playground.iOS.Styles
{
    [PresentationStyle(Id = nameof(BlurOverScreenPresentationStyle))]
    public class BlurOverScreenPresentationStyle : PresentationArgsBase
    {
        public override UIModalPresentationStyle PresentationStyle => UIModalPresentationStyle.BlurOverFullScreen;
    }
}
