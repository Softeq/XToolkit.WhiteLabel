// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Android.Text;
using Java.Lang;

namespace Softeq.XToolkit.Common.Droid.TextFilters
{
    /// <summary>
    ///     This filter will constrain edits not to make text contains forbidden symbols.
    /// </summary>
    public class ForbiddenCharsInputFilter : Object, IInputFilter
    {
        private readonly char[]? _forbiddenCharacters;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ForbiddenCharsInputFilter"/> class.
        /// </summary>
        /// <param name="forbiddenCharacters">Array of characters not allowed in the input. Ignored when null.</param>
        public ForbiddenCharsInputFilter(char[]? forbiddenCharacters)
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
