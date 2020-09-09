// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.iOS.Controls;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class UITextViewExtensions
    {
        public static void SetFilter(this UITextField textField, ITextFilter filter)
        {
            textField.ShouldChangeCharacters = (field, range, replacementString) =>
            {
                return filter.ShouldChangeText(field, field.Text, range, replacementString);
            };
        }

        public static void SetFilter(this UITextView textField, ITextFilter filter)
        {
            textField.ShouldChangeText = (field, range, replacementString) =>
            {
                return filter.ShouldChangeText(field, field.Text, range, replacementString);
            };
        }
    }
}
