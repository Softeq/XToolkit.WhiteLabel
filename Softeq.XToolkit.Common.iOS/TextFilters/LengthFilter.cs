// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.TextFilters
{
    /// <summary>
    ///     This filter will constrain edits not to make the length of the text greater than the specified length.
    /// </summary>
    public class LengthFilter : ITextFilter
    {
        private readonly int _maxLength;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LengthFilter"/> class.
        /// </summary>
        /// <param name="maxLength">Maximum length of text allowed.</param>
        public LengthFilter(int maxLength)
        {
            _maxLength = maxLength;
        }

        /// <summary>
        ///     Gets maximum length enforced by this filter.
        /// </summary>
        public int MaxLength => _maxLength;

        /// <summary>
        ///     Gets or sets a value indicating whether extra characters will be removed.
        /// </summary>
        public bool IsCharacterOverwritingEnabled { get; set; }

        /// <inheritdoc cref="ITextFilter" />
        public virtual bool ShouldChangeText(UIResponder responder, string? oldText, NSRange range, string inputText)
        {
            var newText = oldText ?? string.Empty;

            newText = newText
                .Remove((int) range.Location, (int) range.Length)
                .Insert((int) range.Location, inputText);

            if (newText.Length <= _maxLength)
            {
                return true;
            }

            if (IsCharacterOverwritingEnabled)
            {
                newText = newText.Substring(0, _maxLength);
                var cursorPosition = (nint) Math.Min(range.Location + inputText.Length, newText.Length);

                ChangeSelectedRange(responder, cursorPosition, newText);
            }

            return false;
        }

        protected virtual void ChangeSelectedRange(UIResponder responder, nint cursorPosition, string newText)
        {
            switch (responder)
            {
                case UITextField textField:
                    ChangeSelectedRange(textField, cursorPosition, newText);
                    break;

                case UITextView textView:
                    ChangeSelectedRange(textView, cursorPosition, newText);
                    break;
            }
        }

        protected virtual void ChangeSelectedRange(UITextField textField, nint cursorPosition, string newText)
        {
            textField.Text = newText;
            var fieldCursorPosition = textField.GetPosition(textField.BeginningOfDocument, cursorPosition);
            textField.SelectedTextRange = textField.GetTextRange(fieldCursorPosition, fieldCursorPosition);
        }

        protected virtual void ChangeSelectedRange(UITextView textView, nint cursorPosition, string newText)
        {
            textView.Text = newText;
            textView.SelectedRange = new NSRange(cursorPosition, 0);
        }
    }
}
