using System;
using Softeq.XToolkit.WhiteLabel.iOS.Dialogs;
using UIKit;

namespace Playground.iOS.Resources
{
    [PresentationStyle(Id = nameof(FormSheetPresentationStyle))]
    public class FormSheetPresentationStyle : PresentationArgsBase
    {
        public override UIModalPresentationStyle PresentationStyle => UIModalPresentationStyle.FormSheet;
    }
}
