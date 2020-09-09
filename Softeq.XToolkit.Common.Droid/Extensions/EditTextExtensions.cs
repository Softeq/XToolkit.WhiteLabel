// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Android.Text;
using Android.Widget;
using Java.Lang;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extensions related to <see cref="T:Android.Widget.EditText" />.
    /// </summary>
    public static class EditTextExtensions
    {
        /// <summary>
        ///     Allows to move cursor to the end of text in EditText when it gains focus.
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
        ///     Allows applying multiple input filters to the EditText.
        /// </summary>
        /// <param name="editText">Edit text.</param>
        /// <param name="filters">Array of filters.</param>
        public static void SetFilters(this EditText editText, params IInputFilter[] filters)
        {
            editText.SetFilters(filters);
        }

        /// <summary>
        ///     This filter will constrain edits not to make text contains forbidden symbols.
        /// </summary>
        public class ForbiddenCharsInputFilter : Object, IInputFilter
        {
            private readonly char[] _forbiddenCharacters;

            /// <summary>
            ///     This filter will constrain edits not to make text contains forbidden symbols.
            /// </summary>
            /// <param name="forbiddenCharacters">Char array of characters not allowed in the input. Ignored when null.</param>
            public ForbiddenCharsInputFilter(char[] forbiddenCharacters)
            {
                _forbiddenCharacters = forbiddenCharacters;
            }

            public ICharSequence? FilterFormatted(ICharSequence source, int start, int end, ISpanned dest, int dstart, int dend)
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
