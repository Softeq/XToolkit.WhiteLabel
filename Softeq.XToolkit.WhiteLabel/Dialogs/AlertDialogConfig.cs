// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     Presents an alert dialog to the application user with a single cancel button.
    /// </summary>
    public class AlertDialogConfig
    {
        public AlertDialogConfig(string title, string message, string closeButtonText)
        {
            Title = title;
            Message = message;
            CloseButtonText = closeButtonText;
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
        ///     Text to be displayed on the close button.
        /// </summary>
        public string CloseButtonText { get; set; }
    }
}
