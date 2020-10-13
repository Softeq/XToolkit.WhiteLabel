// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class DialogsServiceExtensions
    {
        /// <summary>
        ///     Creates a <see cref="DialogFluentNavigator{TViewModel}"/>
        ///     from the specified <see cref="IDialogsService"/>.
        /// </summary>
        /// <typeparam name="T">Type of ViewModel.</typeparam>
        /// <param name="dialogsService">Instance of <see cref="IDialogsService"/>.</param>
        /// <returns>
        ///     Instance of <see cref="DialogFluentNavigator{TViewModel}"/>
        ///     created from the specified <see cref="IDialogsService"/>.
        /// </returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="dialogsService"/> cannot be <see langword="null"/>.
        /// </exception>
        public static DialogFluentNavigator<T> For<T>(this IDialogsService dialogsService)
            where T : IDialogViewModel
        {
            if (dialogsService == null)
            {
                throw new ArgumentNullException(nameof(dialogsService));
            }

            return new DialogFluentNavigator<T>(dialogsService);
        }
    }
}
