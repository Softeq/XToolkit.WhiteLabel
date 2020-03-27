// Developed by Softeq Development Corporation
// http://www.softeq.com

// ReSharper disable UnusedAutoPropertyAccessor.Global
namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     Presents an alert dialog to the application user with an accept and a cancel button.
    /// </summary>
    public class ConfirmDialogConfig
    {
        public ConfirmDialogConfig(
            string title,
            string message,
            string acceptButtonText,
            string cancelButtonText)
        {
            Title = title;
            Message = message;
            AcceptButtonText = acceptButtonText;
            CancelButtonText = cancelButtonText;
        }

        public ConfirmDialogConfig()
        {
            Title = string.Empty;
            Message = string.Empty;
            AcceptButtonText = string.Empty;
        }

        /// <summary>
        ///     The title of the alert dialog.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        ///     The body text of the alert dialog.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        ///     Text to be displayed on the 'Accept' button.
        /// </summary>
        public string AcceptButtonText { get; set; }

        /// <summary>
        ///     Text to be displayed on the 'Cancel' button.
        /// </summary>
        public string? CancelButtonText { get; set; }

        /// <summary>
        ///     Declares that 'Accept' button should look destructive on UI.
        /// </summary>
        public bool IsDestructive { get; set; }
    }
}
