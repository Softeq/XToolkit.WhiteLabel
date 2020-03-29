// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Text;
using Java.Lang;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        ///     Allows to apply multiple spans to the whole string
        /// </summary>
        /// <returns>The spannable string with all spans apllied.</returns>
        /// <param name="unformattedString">Unformatted string.</param>
        /// <param name="spans">An array of spans.</param>
        public static SpannableString FormatSpannable(this string unformattedString, params Object[] spans)
        {
            var formattedString = new SpannableString(unformattedString);
            foreach (var span in spans)
            {
                formattedString.SetSpan(span, 0, unformattedString.Length, SpanTypes.ExclusiveExclusive);
            }

            return formattedString;
        }

        /// <summary>
        ///     Allows to apply multiple spans to part of the string
        /// </summary>
        /// <returns>The spannable string with all spans apllied.</returns>
        /// <param name="unformattedString">Unformatted string.</param>
        /// <param name="startingIndex">Starting index from which spans shall be applied.</param>
        /// <param name="length">Length of range to which spans shall be applied.</param>
        /// <param name="spans">An array of spans.</param>
        public static SpannableString FormatSpannable(this string unformattedString, int startingIndex, int length,
            params Object[] spans)
        {
            startingIndex = startingIndex < 0 ? 0 : startingIndex;
            length = startingIndex + length >= unformattedString.Length
                ? unformattedString.Length - startingIndex - 1
                : length;

            var formattedString = new SpannableString(unformattedString);
            foreach (var span in spans)
            {
                formattedString.SetSpan(span, startingIndex, length, SpanTypes.ExclusiveExclusive);
            }

            return formattedString;
        }
    }
}
