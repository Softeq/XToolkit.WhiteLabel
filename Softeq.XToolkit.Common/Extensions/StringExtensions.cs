// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Softeq.XToolkit.Common.Extensions
{
    /// <summary>
    ///     Extension methods for <see cref="T:System.String"/>.
    /// </summary>
    public static class StringExtensions
    {
        private const string LinkPattern = @"[a-z]+://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";

        /// <summary>
        ///     Finds links in specified string.
        /// </summary>
        /// <returns>Strings that match the link pattern (xxx://xxxxxx).</returns>
        /// <param name="str">The string to find links in.</param>
        public static IEnumerable<Capture> FindLinks(this string str)
        {
            foreach (Capture match in Regex.Matches(str, LinkPattern, RegexOptions.IgnoreCase))
            {
                yield return match;
            }
        }

        /// <summary>
        ///     Forms a string with initials for a given name string. It can be used as text for avatar view for instance.
        ///     <para/>
        ///     <a href="https://stackoverflow.com/a/28373431/5416939">See this link for more details</a>
        /// </summary>
        /// <returns>The initials of the name.</returns>
        /// <param name="fullName">Full name.</param>
        public static string GetInitialsForName(this string fullName)
        {
            var initials = string.Empty;
            if (!string.IsNullOrEmpty(fullName))
            {
                // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
                initials = Regex.Replace(fullName, @"[\p{P}\p{S}\p{C}\p{N}]+", string.Empty);

                // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
                initials = Regex.Replace(initials, @"\p{Z}+", " ");

                // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
                initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", string.Empty, RegexOptions.IgnoreCase);

                // Extract up to 2 initials from the remaining cleaned name.
                initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))*(?:(\p{L})\p{L}*)?)?$", "$1$2")
                    .Trim();

                if (initials.Length > 2)
                {
                    // Worst case scenario, everything failed, just grab the first two letters of what we have left.
                    initials = initials.Substring(0, 2);
                }
            }

            return initials.ToUpperInvariant();
        }

        /// <summary>
        ///     Converts the first letter of the specified string to uppercase.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>
        ///     A copy of this <see cref="T:System.String"/> object
        ///     with the first letter converted to uppercase.
        /// </returns>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="str"/> parameter cannot be <see langword="null"/> or empty.
        /// </exception>
        public static string CapitalizeFirstLetter(this string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                throw new ArgumentException("The argument can't be null or empty.", nameof(str));
            }

            var chars = str.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }

        /// <summary>
        ///     Remove empty lines from the input string.
        /// </summary>
        /// <param name="str">Input string.</param>
        /// <returns>
        ///     A copy of this <see cref="T:System.String"/> object with no empty lines in it.
        /// </returns>
        public static string RemoveEmptyLines(this string str)
        {
            return Regex.Replace(str, @"[\r\n]*^\s*$[\r\n]*", string.Empty, RegexOptions.Multiline);
        }

        /// <summary>
        ///     Converts the string representation of a number in the CurrentCulture format to its double-precision floating-point number equivalent.
        ///     A return value indicates whether the conversion succeeded or failed.
        /// </summary>
        /// <param name="str">A string containing a number to convert.</param>
        /// <param name="result">
        ///     When this method returns, contains a double-precision floating-point number equivalent
        ///     of the numeric value or symbol contained in <paramref name="s" />,
        ///     if the conversion succeeded, or zero if the conversion failed.
        ///     The conversion fails if the <paramref name="s" /> parameter is <see langword="null" />
        ///     or <see cref="F:System.String.Empty" />, is not in a format compliant with <paramref name="style" />,
        ///     represents a number less than <see cref="F:System.SByte.MinValue" /> or greater than <see cref="F:System.SByte.MaxValue" />,
        ///     or if <paramref name="style" /> is not a valid combination of <see cref="T:System.Globalization.NumberStyles" /> enumerated constants.
        ///     This parameter is passed uninitialized; any value originally supplied in <paramref name="result" /> will be overwritten.
        /// </param>
        /// <returns>
        ///     <see langword="true" /> if <paramref name="str" /> was converted successfully; otherwise, <see langword="false" />.</returns>
        /// </returns>
        public static bool TryParseDouble(this string str, out double? result)
        {
            if (double.TryParse(str, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.CurrentCulture, out var number))
            {
                result = number;
                return true;
            }

            result = null;
            return string.IsNullOrEmpty(str);
        }
    }
}
