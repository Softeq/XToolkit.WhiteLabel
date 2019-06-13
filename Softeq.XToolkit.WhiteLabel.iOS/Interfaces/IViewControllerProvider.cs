// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Interfaces
{
    public interface IViewControllerProvider
    {
        UIViewController GetRootViewController(UIViewController controller);
    }
}