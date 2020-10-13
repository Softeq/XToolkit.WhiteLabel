// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class FrameNavigationServiceExtensions
    {
        /// <summary>
        ///     Creates a <see cref="FrameFluentNavigator{TViewModel}"/> from the specified <see cref="IFrameNavigationService"/>.
        /// </summary>
        /// <typeparam name="T">Type of ViewModel.</typeparam>
        /// <param name="frameNavigationService">Instance of <see cref="IFrameNavigationService"/>.</param>
        /// <returns>
        ///     Instance of <see cref="FrameFluentNavigator{TViewModel}"/> created from the specified <see cref="IFrameNavigationService"/>.
        /// </returns>
        public static FrameFluentNavigator<T> For<T>(this IFrameNavigationService frameNavigationService)
            where T : IViewModelBase
        {
            return new FrameFluentNavigator<T>(frameNavigationService);
        }
    }
}
