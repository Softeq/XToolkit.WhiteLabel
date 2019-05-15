// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Controls
{
    public class LengthValidator
    {
        private readonly UIResponder _textField;

        public LengthValidator(UIResponder textField, int maxLength = int.MaxValue)
        {
            _textField = textField;
            MaxLength = maxLength;
            if (_textField is UITextField)
            {
                ((UITextField) _textField).ShouldChangeCharacters += OnChangeTextField;
            }
            else if (_textField is UITextView)
            {
                ((UITextView) _textField).ShouldChangeText += OnChangeTextView;
            }
        }

        public int MaxLength { get; set; }

        private bool OnChangeTextField(UITextField textField, NSRange range, string replacementString)
        {
            return ShouldChangeValue(textField.Text, range, replacementString);
        }

        private bool OnChangeTextView(UITextView textView, NSRange range, string text)
        {
            return ShouldChangeValue(textView.Text, range, text);
        }

        private bool ShouldChangeValue(string oldText, NSRange range, string inputText)
        {
            var newText = oldText ?? string.Empty;
            newText = newText.Remove((int) range.Location, (int) range.Length)
                .Insert((int) range.Location, inputText);
            if (newText.Length > MaxLength)
            {
                newText = newText.Substring(0, MaxLength);
                var cursorPosition = (nint) Math.Min(range.Location + inputText.Length, newText.Length);

                if (_textField is UITextField)
                {
                    var castedField = _textField as UITextField;
                    castedField.Text = newText;
                    var fieldCursorPosition = castedField.GetPosition(castedField.BeginningOfDocument, cursorPosition);
                    castedField.SelectedTextRange = castedField.GetTextRange(fieldCursorPosition, fieldCursorPosition);
                }
                else if (_textField is UITextView)
                {
                    var castedView = _textField as UITextView;
                    castedView.Text = newText;
                    castedView.SelectedRange = new NSRange(cursorPosition, 0);
                }

                return false;
            }

            return true;
        }
    }
}