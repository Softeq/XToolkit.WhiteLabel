// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Android.Text;
using Android.Widget;
using Java.Lang;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    public static class EditTextExtensions
    {
        /// <summary>
        /// Allows to move cursor to the end of text in EditText when it gains focus
        /// </summary>
        /// <param name="editText">Edit text.</param>
        public static void KeepFocusAtTheEndOfField(this EditText editText)
        {
            editText.FocusChange += (sender, e) =>
            {
                editText.SetSelection(editText.Text.Length);
            };
        }

        /// <summary>
        /// Allows to apply limitations of length and/or input symbols to an EditText
        /// </summary>
        /// <param name="editText">Edit text.</param>
        /// <param name="maxLength">Maximum length of text allowed. Ignored when 0 or negative</param>
        /// <param name="forbiddenCharacters">Char array of characters not allowed in the input. Ignored when null</param>
        public static void SetMaxLengthWithForbiddenSymbols(this EditText editText, int maxLength = 0, char[] forbiddenCharacters = null)
        {
            var filters = new List<IInputFilter>();

            if (maxLength > 0)
            {
                filters.Add(new InputFilterLengthFilter(maxLength));
            }

            if (forbiddenCharacters != null)
            {
                filters.Add(new ForbiddenCharsInputFilter(forbiddenCharacters));
            }

            editText.SetFilters(filters.ToArray());
        }

        private class ForbiddenCharsInputFilter : Object, IInputFilter
        {
            private readonly char[] _forbiddenCharacters;

            public ForbiddenCharsInputFilter(char[] forbiddenCharacters)
            {
                _forbiddenCharacters = forbiddenCharacters;
            }

            public ICharSequence FilterFormatted(ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
            {
                if (source != null && _forbiddenCharacters != null)
                {
                    var intersection = source.ToArray().Intersect(_forbiddenCharacters);
                    if (intersection != null && intersection.Any())
                    {
                        return new String(string.Empty);
                    }
                }
                return null;
            }
        }
    }
}
