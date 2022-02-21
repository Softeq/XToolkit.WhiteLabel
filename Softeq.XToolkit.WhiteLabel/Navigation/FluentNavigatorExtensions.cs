// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class FluentNavigatorExtensions
    {
        /// <summary>
        ///     Creates a <see cref="DialogFluentNavigator{TViewModel}"/>
        ///     from the specified <see cref="IDialogsService"/>.
        /// </summary>
        /// <typeparam name="TViewModel">Type of ViewModel.</typeparam>
        /// <param name="dialogsService">Instance of <see cref="IDialogsService"/>.</param>
        /// <returns>
        ///     Instance of <see cref="DialogFluentNavigator{TViewModel}"/>
        ///     created from the specified <see cref="IDialogsService"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="dialogsService"/> cannot be <see langword="null"/>.
        /// </exception>
        public static DialogFluentNavigator<TViewModel> For<TViewModel>(this IDialogsService dialogsService)
            where TViewModel : class, IDialogViewModel
        {
            if (dialogsService == null)
            {
                throw new ArgumentNullException(nameof(dialogsService));
            }

            return new DialogFluentNavigator<TViewModel>(dialogsService);
        }

        /// <summary>
        ///     Creates a <see cref="FrameFluentNavigator{TViewModel}"/>
        ///     from the specified <see cref="IFrameNavigationService"/>.
        /// </summary>
        /// <typeparam name="TViewModel">Type of ViewModel.</typeparam>
        /// <param name="frameNavigationService">Instance of <see cref="IFrameNavigationService"/>.</param>
        /// <returns>
        ///     Instance of <see cref="FrameFluentNavigator{TViewModel}"/>
        ///     created from the specified <see cref="IFrameNavigationService"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="frameNavigationService"/> cannot be <see langword="null"/>.
        /// </exception>
        public static FrameFluentNavigator<TViewModel> For<TViewModel>(this IFrameNavigationService frameNavigationService)
            where TViewModel : IViewModelBase
        {
            if (frameNavigationService == null)
            {
                throw new ArgumentNullException(nameof(frameNavigationService));
            }

            return new FrameFluentNavigator<TViewModel>(frameNavigationService);
        }

        /// <summary>
        ///     Creates a <see cref="PageFluentNavigator{TViewModel}"/>
        ///     from the specified <see cref="IPageNavigationService"/>.
        /// </summary>
        /// <typeparam name="TViewModel">Type of ViewModel.</typeparam>
        /// <param name="pageNavigationService">Instance of <see cref="IPageNavigationService"/>.</param>
        /// <returns>
        ///     Instance of <see cref="PageFluentNavigator{TViewModel}"/>
        ///     created from the specified <see cref="IFrameNavigationService"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="pageNavigationService"/> cannot be <see langword="null"/>.
        /// </exception>
        public static PageFluentNavigator<TViewModel> For<TViewModel>(this IPageNavigationService pageNavigationService)
            where TViewModel : IViewModelBase
        {
            if (pageNavigationService == null)
            {
                throw new ArgumentNullException(nameof(pageNavigationService));
            }

            return new PageFluentNavigator<TViewModel>(pageNavigationService);
        }
    }
}
