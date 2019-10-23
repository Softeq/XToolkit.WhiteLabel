using System;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using UIKit;

namespace Playground.iOS.Styles
{
    [PresentationStyle(Id = nameof(OverFullScreenPresentationStyle))]
    public class OverFullScreenPresentationStyle : PresentationArgsBase
    {
        public override UIModalPresentationStyle PresentationStyle => UIModalPresentationStyle.OverFullScreen;
    }
}
