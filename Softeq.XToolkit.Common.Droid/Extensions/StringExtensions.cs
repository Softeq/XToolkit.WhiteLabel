// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.Text;
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
        /// <returns>The spannable string with all spans apllied.</returns>
        /// <param name="unformattedString">Unformatted string.</param>
        /// <param name="startingIndex">Starting index from which spans shall be applied.</param>
        /// <param name="length">Length of range to which spans shall be applied.</param>
        /// <param name="spans">An array of spans.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="unformattedString"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        /// /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="startingIndex"/> parameter must be less than the size of the string and greater or equal than 0.
        ///     - and -
        ///     <paramref name="length"/> parameter must be less than the size of the string and greater or equal than 0.
        ///     - and -
        ///     sum of these params must be less than the size of the string.
        /// </exception>
        public static SpannableString FormatSpannable(
            this string unformattedString,
            int startingIndex,
            int length,
            params Object[] spans)
        {
            if (unformattedString == null)
            {
                throw new ArgumentNullException($"{nameof(unformattedString)} cannot be null");
            }

            if (spans.Length == 0)
            {
                throw new ArgumentException($"{nameof(spans)} must be non-empty");
            }

            if (startingIndex < 0 || startingIndex >= unformattedString.Length)
            {
                throw new ArgumentOutOfRangeException($"{nameof(startingIndex)} must be > 0 and < {unformattedString.Length}");
            }

            if (length <= 0 || length > unformattedString.Length)
            {
                throw new ArgumentOutOfRangeException($"{nameof(length)} must be > 0 and < {unformattedString.Length}");
            }

            if (startingIndex + length > unformattedString.Length)
            {
                throw new ArgumentOutOfRangeException($"{nameof(startingIndex)} + {nameof(length)} must be <= {unformattedString.Length}");
            }

            var formattedString = new SpannableString(unformattedString);
            foreach (var span in spans)
            {
                formattedString.SetSpan(span, startingIndex, startingIndex + length, SpanTypes.ExclusiveExclusive);
            }

            return formattedString;
        }

        /// <summary>
        ///     Allows to apply multiple spans to the whole string.
        /// </summary>
        /// <returns>The spannable string with all spans apllied.</returns>
        /// <param name="unformattedString">Unformatted string.</param>
        /// <param name="spans">An array of spans.</param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="unformattedString"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        public static SpannableString FormatSpannable(this string unformattedString, params Object[] spans)
        {
            if (unformattedString == null)
            {
                throw new ArgumentNullException($"{nameof(unformattedString)} cannot be null");
            }

            return unformattedString.FormatSpannable(0, unformattedString.Length, spans);
        }
    }
}
