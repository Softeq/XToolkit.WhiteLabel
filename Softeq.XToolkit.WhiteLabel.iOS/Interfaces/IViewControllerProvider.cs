// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Interfaces
{
    public interface IViewControllerProvider
    {
        /// <summary>
        /// Returns top ViewController except modals.
        /// </summary>
        /// <param name="controller">RootViewController.</param>
        /// <returns></returns>
        UIViewController GetTopViewController(UIViewController controller);
    }
}