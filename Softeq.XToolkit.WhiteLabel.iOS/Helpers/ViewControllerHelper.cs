// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Common.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Helpers
{
    /// <summary>
    ///     Contains methods for instantiating ViewControllers.
    /// </summary>
    public static class ViewControllerHelper
    {
        /// <summary>
        ///     Method for safety attempt to create UIViewController from storyboard
        ///     otherwise creates a new instance of <paramref name="viewControllerType" />.
        /// </summary>
        /// <param name="storyboardName">Storyboard name.</param>
        /// <param name="viewControllerType">Type of ViewController on Storyboard.</param>
        /// <param name="logger">Instance of <see cref="ILogger"/>.</param>
        /// <returns>Target instance of <paramref name="viewControllerType"/>.</returns>
        public static UIViewController? TryCreateViewController(string storyboardName, Type viewControllerType, ILogger logger)
        {
            UIViewController? newViewController = null;

            try
            {
                Execute.OnUIThread(() =>
                {
                    newViewController = CreateViewControllerFromStoryboard(storyboardName, viewControllerType.Name)
                                        ?? (UIViewController) Activator.CreateInstance(viewControllerType);
                });
            }
            catch (Exception ex)
            {
                logger.Warn(ex);
            }

            return newViewController;
        }

        /// <summary>
        ///     Method for instantiating UIViewController from Storyboard if found, otherwise returns NULL.
        /// </summary>
        /// <param name="storyboardName">Storyboard name.</param>
        /// <param name="viewControllerName">Name of ViewController on Storyboard.</param>
        /// <returns>New instance of UIViewController.</returns>
        public static UIViewController? CreateViewControllerFromStoryboard(string storyboardName, string viewControllerName)
        {
            if (NSBundle.MainBundle.PathForResource(storyboardName, "storyboardc") == null)
            {
                return null;
            }

            var storyboard = UIStoryboard.FromName(storyboardName, null);
            return storyboard?.InstantiateViewController(viewControllerName);
        }
    }
}
