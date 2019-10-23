using System;
using Playground.iOS.Resources;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using UIKit;

namespace Playground.iOS.Styles
{
    [PresentationStyle(Id = nameof(FullScreenPresentationStyle))]
    public class FullScreenPresentationStyle : PresentationArgsBase
    {
        public override UIModalPresentationStyle PresentationStyle => UIModalPresentationStyle.FullScreen;
    }
}
