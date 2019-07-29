// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Common.iOS.Extensions
{
    public static class UITextFieldExtensions
    {
        // TODO: possibly should be merged with LengthValidator

        /// <summary>
        ///     Allows to apply limitations of length and/or input symbols on a UITextField
        ///     <para>Differently from LengthValidator this method will ignore input if it's invalid</para>
        ///     <para>Should be returned from ShouldChangeCharacters callback</para>
        /// </summary>
        /// <returns><c>true</c>, if change characters should be applied, <c>false</c> otherwise.</returns>
        /// <param name="textField">Text field.</param>
        /// <param name="range">Range being replaced.</param>
        /// <param name="replacementString">Replacement string.</param>
        /// <param name="maxLength">Maximum length of text allowed. Ignored when 0 or negative</param>
        /// <param name="forbiddenCharacters">Char array of characters not allowed in the input. Ignored when null</param>
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
