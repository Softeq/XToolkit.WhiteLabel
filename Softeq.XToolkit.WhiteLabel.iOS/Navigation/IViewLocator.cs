// Developed by Softeq Development Corporation
// http://www.softeq.com

using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    /// <summary>
    ///     iOS platform-specific interface.
    ///     Provides methods for getting <see cref="T:UIKit.UIViewController"/> instances.
    /// </summary>
    public interface IViewLocator
    {
        /// <summary>
        ///     Get view controller related to <paramref name="viewModel"/>.
        /// </summary>
        /// <param name="viewModel">An instance of View-Model.</param>
        /// <returns>An instance of related <see cref="T:UIKit.UIViewController"/>.</returns>
        UIViewController GetView(object viewModel);

        /// <summary>
        ///     Get top view controller.
        /// </summary>
        /// <returns>Top <see cref="T:UIKit.UIViewController"/>.</returns>
        UIViewController GetTopViewController();
    }
}
