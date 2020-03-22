namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    public interface IDialogConfig
    {
    }

    // ReSharper disable once UnusedTypeParameter
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
        public string? CancelButtonText { get; set; }

        /// <summary>
        ///     Declares that 'Accept' button should be looks destructive on UI.
        /// </summary>
        public bool IsDestructive { get; set; }
    }


    /// <summary>
    ///     Displays a native platform action sheet,
    ///     allowing the application user to choose from several buttons.
    /// </summary>
    public class ActionSheetDialogConfig : IDialogConfig<string>
    {
        /// <summary>
        ///     Title of the displayed action sheet.
        /// </summary>
        public string? Title { get; set; }

        /// <summary>
        ///     Text to be displayed in the 'Cancel' button.
        ///     Can be null to hide the cancel action.
        /// </summary>
        public string? CancelButtonText { get; set; }

        /// <summary>
        ///     Text to be displayed in the 'Destruct' button.
        ///     Can be null to hide the destructive option.
        /// </summary>
        public string? DestructButtonText { get; set; }

        /// <summary>
        ///     Text labels for additional buttons.
        /// </summary>
        public string[] OptionButtons { get; set; }
    }


}
