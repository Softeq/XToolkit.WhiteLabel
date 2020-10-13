// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class FrameNavigationServiceExtensions
    {
        /// <summary>
        ///     Creates a <see cref="FrameFluentNavigator{TViewModel}"/>
        ///     from the specified <see cref="IFrameNavigationService"/>.
        /// </summary>
        /// <typeparam name="T">Type of ViewModel.</typeparam>
        /// <param name="frameNavigationService">Instance of <see cref="IFrameNavigationService"/>.</param>
        /// <returns>
        ///     Instance of <see cref="FrameFluentNavigator{TViewModel}"/>
        ///     created from the specified <see cref="IFrameNavigationService"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="frameNavigationService"/> cannot be <see langword="null"/>.
        /// </exception>
        public static FrameFluentNavigator<T> For<T>(this IFrameNavigationService frameNavigationService)
            where T : IViewModelBase
        {
            if (frameNavigationService == null)
            {
                throw new ArgumentNullException(nameof(frameNavigationService));
            }

            return new FrameFluentNavigator<T>(frameNavigationService);
        }
    }
}
