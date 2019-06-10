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
        public static void KeepFocusAtTheEndOfField(this EditText editText)
        {
            editText.FocusChange += (sender, e) =>
            {
                editText.SetSelection(editText.Text.Length);
            };
        }

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
