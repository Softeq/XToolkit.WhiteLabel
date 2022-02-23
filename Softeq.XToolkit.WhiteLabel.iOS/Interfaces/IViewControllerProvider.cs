// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Interfaces
{
    /// <summary>
    ///     Provides methods for getting <see cref="T:UIKit.UIViewController"/> instances.
    /// </summary>
    public interface IViewControllerProvider
    {
        /// <summary>
        ///     Returns top <see cref="T:UIKit.UIViewController"/> except modals.
        /// </summary>
        /// <param name="controller">RootViewController.</param>
        /// <returns>Top ViewController.</returns>
        UIViewController GetTopViewController(UIViewController controller);
    }
}
