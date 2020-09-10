// Developed by Softeq Development Corporation
// http://www.softeq.com

using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.TextFilters
{
    /// <summary>
    ///     Filters can be attached to UI views to constrain the changes that can be made to them.
    /// </summary>
    public interface ITextFilter
    {
        /// <summary>
        ///    Returns value whether to change the specified text.
        /// </summary>
        /// <param name="responder">The view containing the text.</param>
        /// <param name="oldText">Current text that view contains.</param>
        /// <param name="range">The range of characters to be replaced.</param>
        /// <param name="replacementString">
        ///     The replacement string for the specified range.
        ///     During typing, this parameter normally contains only the single new character that was typed,
        ///     but it may contain more characters if the user is pasting text.
        ///     When the user deletes one or more characters, the replacement string is empty.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the specified text range should be replaced;
        ///     otherwise, <see langword="false"/> to keep the old text.
        /// </returns>
        bool ShouldChangeText(
            UIResponder responder,
            string? oldText,
            NSRange range,
            string replacementString);
    }
}
