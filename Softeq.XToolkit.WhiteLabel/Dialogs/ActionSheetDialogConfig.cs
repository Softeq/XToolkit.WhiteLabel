// Developed by Softeq Development Corporation
// http://www.softeq.com

namespace Softeq.XToolkit.WhiteLabel.Dialogs
{
    /// <summary>
    ///     Configuration of an action sheet dialog
    ///     that allows user to choose from several options.
    /// </summary>
    public class ActionSheetDialogConfig
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionSheetDialogConfig"/> class.
        /// </summary>
        /// <param name="optionButtons">The text to be displayed on the option buttons.</param>
        /// <param name="title">Dialog title.</param>
        /// <param name="cancelButtonText">The text to be displayed on the 'Cancel' button.</param>
        /// <param name="destructButtonText">The text to be displayed on the 'Destruct' button.</param>
        public ActionSheetDialogConfig(
            string[]? optionButtons,
            string? title = null,
            string? cancelButtonText = null,
            string? destructButtonText = null)
        {
            OptionButtons = optionButtons ?? new string[] { };
            Title = title;
            CancelButtonText = cancelButtonText;
            DestructButtonText = destructButtonText;
        }

        /// <summary>
        ///     Gets the dialog title.
        ///     Can be <see langword="null"/> to hide the title.
        /// </summary>
        public string? Title { get; }

        /// <summary>
        ///     Gets the text to be displayed on the 'Cancel' button.
        ///     Can be <see langword="null"/> to hide the 'Cancel' button.
        /// </summary>
        public string? CancelButtonText { get; }

        /// <summary>
        ///     Gets the text to be displayed on the 'Destruct' button.
        ///     Can be <see langword="null"/> to hide the 'Destruct' button.
        /// </summary>
        public string? DestructButtonText { get; }

        /// <summary>
        ///    Gets the text to be displayed on the option buttons.
        /// </summary>
        public string[] OptionButtons { get; }
    }
}
