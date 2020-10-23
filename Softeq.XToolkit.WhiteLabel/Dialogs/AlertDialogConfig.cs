// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     Configuration of an alert dialog with a single 'Close' button.
    /// </summary>
    public class AlertDialogConfig
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AlertDialogConfig"/> class.
        /// </summary>
        /// <param name="title">Dialog title.</param>
        /// <param name="message">Dialog message.</param>
        /// <param name="closeButtonText">The text to be displayed on the 'Close' button.</param>
        public AlertDialogConfig(string title, string message, string closeButtonText)
        {
            Title = title ?? throw new ArgumentNullException(nameof(title));
            Message = message ?? throw new ArgumentNullException(nameof(message));
            CloseButtonText = closeButtonText ?? throw new ArgumentNullException(nameof(closeButtonText));
        }

        /// <summary>
        ///    Gets the dialog title.
        /// </summary>
        public string Title { get; }

        /// <summary>
        ///     Gets the dialog message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        ///     Gets the text to be displayed on the 'Close' button.
        /// </summary>
        public string CloseButtonText { get; }
    }
}
