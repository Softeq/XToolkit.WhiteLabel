// Developed by Softeq Development Corporation
// http://www.softeq.com

// ReSharper disable UnusedAutoPropertyAccessor.Global
using System;

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     Configuration of an alert dialog with 'Confirm' and 'Cancel' buttons.
    /// </summary>
    public class ConfirmDialogConfig
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ConfirmDialogConfig"/> class.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        /// <param name="message">Dialog message.</param>
        /// <param name="acceptButtonText">The text to be displayed on the 'Accept' button.</param>
        /// <param name="cancelButtonText">The text to be displayed on the 'Cancel' button.</param>
        /// <param name="isDestructive">
        ///     Value indicating whether the 'Accept' button should look destructive on UI.
        /// </param>
        /// <param name="imageName">
        ///     The name of the dialog image or <see langword="null"/> if no image should be used.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="title"/>
        ///     and <paramref name="message"/>
        ///     and <paramref name="acceptButtonText"/>
        ///     cannot be null.
        /// </exception>
        public ConfirmDialogConfig(
            string title,
            string message,
            string acceptButtonText,
            string? cancelButtonText = null,
            bool isDestructive = false,
            string? imageName = null)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            AcceptButtonText = acceptButtonText ?? throw new ArgumentNullException(nameof(acceptButtonText));
            CancelButtonText = cancelButtonText;
            IsDestructive = isDestructive;
            ImageName = imageName;
        }

        /// <summary>
        ///     Gets the dialog title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     Gets the dialog message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gets the text to be displayed on the 'Accept' button.
        /// </summary>
        public string AcceptButtonText { get; }

        /// <summary>
        ///     Gets the text to be displayed on the 'Cancel' button.
        ///     Can be <see langword="null"/> to hide the 'Cancel' button.
        /// </summary>
        public string? CancelButtonText { get; }

        /// <summary>
        ///     Gets a value indicating whether
        ///     the 'Accept' button should look destructive on UI.
        /// </summary>
        public bool IsDestructive { get; }

        /// <summary>
        ///     Gets the name of the dialog image or <see langword="null"/> if no image should be used.
        /// </summary>
        public string? ImageName { get; }
    }
}
