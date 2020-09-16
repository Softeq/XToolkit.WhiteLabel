// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Android.Text;
using Android.Widget;
using Java.Lang;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:Android.Widget.EditText"/>.
    /// </summary>
    public static class EditTextExtensions
    {
        /// <summary>
        ///     Allows to move cursor to the end of text in <see cref="T:Android.Widget.EditText" /> when it gains focus.
        /// </summary>
        /// <param name="editText"><see cref="T:Android.Widget.EditText" /> control.</param>
        public static void KeepFocusAtTheEndOfField(this EditText editText)
        {
            editText.FocusChange += (sender, e) =>
            {
                if (editText.Text != null)
                {
                    editText.SetSelection(editText.Text.Length);
                }
            };
        }

        /// <summary>
        ///     Allows applying multiple input filters to the <see cref="T:Android.Widget.EditText" />.
        /// </summary>
        /// <param name="editText"><see cref="T:Android.Widget.EditText" /> control.</param>
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
            ///     Initializes a new instance of the <see cref="ForbiddenCharsInputFilter"/> class.
            /// </summary>
            /// <param name="forbiddenCharacters">Array of characters not allowed in the input. Ignored when null.</param>
            public ForbiddenCharsInputFilter(char[] forbiddenCharacters)
            {
                _forbiddenCharacters = forbiddenCharacters;
            }

            /// <inheritdoc />
            public ICharSequence? FilterFormatted(ICharSequence? source, int start, int end, ISpanned? dest, int dstart, int dend)
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
