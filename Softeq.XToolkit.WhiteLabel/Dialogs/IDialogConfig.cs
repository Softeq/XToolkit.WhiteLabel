namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    public interface IDialogConfig
    {
    }

    public interface IDialogConfig<T> : IDialogConfig
    {
    }

    /// <summary>
    ///     Presents an alert dialog to the application user with a single cancel button.
    /// </summary>
    public class AlertDialogConfig : IDialogConfig
    {
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

    /// <summary>
    ///     Presents an alert dialog to the application user with an accept and a cancel button.
    /// </summary>
    public class ConfirmDialogConfig : IDialogConfig<bool>
    {
        /// <summary>
        ///     The title of the alert dialog.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        ///     The body text of the alert dialog.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        ///     Text to be displayed on the 'Accept' button.
        /// </summary>
        public string AcceptButtonText { get; set; } = "OK";

        /// <summary>
        ///     Text to be displayed on the 'Cancel' button.
        /// </summary>
        public string? CancelButtonText { get; set; } = null;

        public bool IsDestructive { get; set; }
    }
}
