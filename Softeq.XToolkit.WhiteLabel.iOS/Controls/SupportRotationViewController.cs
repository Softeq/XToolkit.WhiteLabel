// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public class SupportRotationViewController : UIViewController
    {
        public override bool ShouldAutorotate()
        {
            if (ChildViewControllers.Length > 0)
            {
                return ChildViewControllers[0].ShouldAutorotate();
            }

            return false;
        }

        public override UIInterfaceOrientationMask GetSupportedInterfaceOrientations()
        {
            if (ChildViewControllers.Length > 0)
            {
                return ChildViewControllers[0].GetSupportedInterfaceOrientations();
            }

            return UIInterfaceOrientationMask.Portrait;
        }
    }
}