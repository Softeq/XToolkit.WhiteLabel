// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Text;
using Softeq.XToolkit.Common.Helpers;
using Object = Java.Lang.Object;

namespace Softeq.XToolkit.Common.Droid.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.String"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Allows to apply multiple spans to part of the string.
        /// </summary>
        /// <returns>The spannable string with all spans applied.</returns>
        /// <param name="unformattedString">Unformatted string.</param>
        /// <param name="textRange">
        ///     Instance containing the starting index and the length of the range to which spans shall be applied.
        /// </param>
        /// <param name="spans">An array of spans.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="unformattedString"/> and <paramref name="textRange"/> parameters cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="spans"/> list cannot be empty.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     Starting index of the <paramref name="textRange"/> must be less than the size of the string.
        ///     - and -
        ///     Length of the <paramref name="textRange"/> must be less than the size of the string.
        ///     - and -
        ///     End index of the <paramref name="textRange"/> must be less than the size of the string.
        /// </exception>
        public static SpannableString FormatSpannable(
            this string unformattedString,
            TextRange textRange,
            params Object[] spans)
        {
            if (unformattedString == null)
            {
                throw new ArgumentNullException(nameof(unformattedString));
            }

            if (textRange == null)
            {
                throw new ArgumentNullException(nameof(textRange));
            }

            if (spans.Length == 0)
            {
                throw new ArgumentException($"{nameof(spans)} must be non-empty");
            }

            if (textRange.Position >= unformattedString.Length)
            {
                throw new ArgumentOutOfRangeException($"{nameof(textRange.Position)} must be < {unformattedString.Length}");
            }

            if (textRange.Length > unformattedString.Length)
            {
                throw new ArgumentOutOfRangeException($"{nameof(textRange.Length)} must be < {unformattedString.Length}");
            }

            if (textRange.Position + textRange.Length > unformattedString.Length)
            {
                throw new ArgumentOutOfRangeException($"{nameof(textRange.Position)} + {nameof(textRange.Length)} must be <= {unformattedString.Length}");
            }

            var formattedString = new SpannableString(unformattedString);
            foreach (var span in spans)
            {
                formattedString.SetSpan(span, textRange.Position, textRange.Position + textRange.Length, SpanTypes.ExclusiveExclusive);
            }

            return formattedString;
        }

        /// <summary>
        ///     Allows to apply multiple spans to the whole string.
        /// </summary>
        /// <returns>The spannable string with all spans applied.</returns>
        /// <param name="unformattedString">Unformatted string.</param>
        /// <param name="spans">An array of spans.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="unformattedString"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        public static SpannableString FormatSpannable(this string unformattedString, params Object[] spans)
        {
            if (unformattedString == null)
            {
                throw new ArgumentNullException(nameof(unformattedString));
            }

            return unformattedString.FormatSpannable(new TextRange(0, unformattedString.Length), spans);
        }
    }
}
