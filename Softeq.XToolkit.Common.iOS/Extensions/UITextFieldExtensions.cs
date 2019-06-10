// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class UITextFieldExtensions
    {
        //Shall be returned from ShouldChangeCharacters callback of UITextField to apply limitations for that text field
        public static bool OnChangeCharactersWithLengthAndCharsLimit(this UITextField textField,
            NSRange range, string replacementString, int maxLength = 0, char[] forbiddenCharacters = null)
        {
            if (range.Length + range.Location > textField.Text.Length)
            {
                return false;
            }

            if (forbiddenCharacters != null)
            {
                var intersection = replacementString.ToCharArray().Intersect(forbiddenCharacters);
                if (intersection != null && intersection.Any())
                {
                    return false;
                }
            }

            if (maxLength > 0)
            {
                var newLength = textField.Text.Length + replacementString.Length - range.Length;
                return newLength <= maxLength;
            }

            return true;
        }
    }
}
