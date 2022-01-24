// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Interfaces
{
    public interface IViewControllerProvider
    {
        /// <summary>
        ///     Returns top ViewController except modals.
        /// </summary>
        /// <param name="controller">RootViewController.</param>
        /// <returns>Top ViewController.</returns>
        UIViewController GetTopViewController(UIViewController controller);
    }
}
