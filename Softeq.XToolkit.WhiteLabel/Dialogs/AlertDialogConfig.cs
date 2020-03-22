// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     Presents an alert dialog to the application user with a single cancel button.
    /// </summary>
    public class AlertDialogConfig : IDialogConfig<object>
    {
        public AlertDialogConfig()
        {
        }

        public AlertDialogConfig(string title, string message, string cancelButtonText)
        {
            Title = title;
            Message = message;
            CancelButtonText = cancelButtonText;
        }

        /// <summary>
        ///     The title of the alert dialog.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        ///     The body text of the alert dialog.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        ///     Text to be displayed on the close button.
        /// </summary>
        public string CancelButtonText { get; set; } = "OK";
    }
}
