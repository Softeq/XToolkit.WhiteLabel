// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     Displays a native platform action sheet,
    ///     allowing the application user to choose from several buttons.
    /// </summary>
    public class ActionSheetDialogConfig
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
        public string[] OptionButtons { get; set; } = new string[] {};
    }
}
