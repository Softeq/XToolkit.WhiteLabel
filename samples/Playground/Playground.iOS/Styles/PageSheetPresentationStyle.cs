using System;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using UIKit;

namespace Playground.iOS.Styles
{
    [PresentationStyle(Id = nameof(PageSheetPresentationStyle))]
    public class PageSheetPresentationStyle : PresentationArgsBase
    {
        public override UIModalPresentationStyle PresentationStyle => UIModalPresentationStyle.PageSheet;
    }
}
