// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public class SupportRotationAlertController : UIAlertController
    {
        public SupportRotationAlertController(string title, string message, UIAlertControllerStyle controllerStyle)
        {
            Title = title;
            Message = message;
            PreferredStyle = controllerStyle;
        }

        public override UIAlertControllerStyle PreferredStyle { get; }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            return UIInterfaceOrientationMask.Portrait;
        }

        public override bool ShouldAutorotate()
        {
            return false;
        }
    }
}