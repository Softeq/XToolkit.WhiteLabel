// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class StringExtensions
    {
        private const string LinkPattern = @"[a-z]+://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";

        public static IEnumerable<Capture> FindLinks(this string self)
        {
            foreach (Capture match in Regex.Matches(self, LinkPattern, RegexOptions.IgnoreCase))
            {
                yield return match;
            }
        }

        /// <summary>
        /// Forms a string with initials for a given name string, can be used as text for avatar view for instance
        /// <para>https://stackoverflow.com/a/28373431/5416939</para>
        /// </summary>
        /// <returns>The initials of the name.</returns>
        /// <param name="fullName">Full name.</param>
        public static string GetInitialsForName(this string fullName)
        {
            string initials = string.Empty;
            if (!string.IsNullOrEmpty(fullName))
            {
                // first remove all: punctuation, separator chars, control chars, and numbers (unicode style regexes)
                initials = Regex.Replace(fullName, @"[\p{P}\p{S}\p{C}\p{N}]+", "");

                // Replacing all possible whitespace/separator characters (unicode style), with a single, regular ascii space.
                initials = Regex.Replace(initials, @"\p{Z}+", " ");

                // Remove all Sr, Jr, I, II, III, IV, V, VI, VII, VIII, IX at the end of names
                initials = Regex.Replace(initials.Trim(), @"\s+(?:[JS]R|I{1,3}|I[VX]|VI{0,3})$", "", RegexOptions.IgnoreCase);

                // Extract up to 2 initials from the remaining cleaned name.
                initials = Regex.Replace(initials, @"^(\p{L})[^\s]*(?:\s+(?:\p{L}+\s+(?=\p{L}))?(?:(\p{L})\p{L}*)?)?$", "$1$2").Trim();

                if (initials.Length > 2)
                {
                    // Worst case scenario, everything failed, just grab the first two letters of what we have left.
                    initials = initials.Substring(0, 2);
                }
            }

            return initials.ToUpperInvariant();
        }
        
        /// <summary>
        /// Returns a copy of this <see cref="T:System.String"></see> object
        /// where first latter converted to uppercase.
        /// </summary>
        /// <param name="value">Input string.</param>
        /// <returns>A copy of this <see cref="T:System.String"></see> object
        /// where first latter converted to uppercase.</returns>
        /// <exception cref="ArgumentException">The argument can't be null or empty.</exception>
        public static string CapitalizeFirstLetter(this string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentException("The argument can't be null or empty.", nameof(value));
            }
            
            var chars = value.ToCharArray();
            chars[0] = char.ToUpper(chars[0]);
            return new string(chars);
        }
        
        /// <summary>
        /// Remove empty lines from the input string.
        /// </summary>
        /// <param name="input">Input string.</param>
        /// <returns></returns>
        public static string RemoveEmptyLines(this string input)
        {
            return Regex.Replace(input, @"[\r\n]*^\s*$[\r\n]*", "", RegexOptions.Multiline);
        }

        /// <summary>
        /// Try parse <see cref="T:System.String">text</see> to <see cref="T:System.Double">double</see> more easily
        /// for CurrentCulture.
        /// </summary>
        /// <param name="text">Input string.</param>
        /// <param name="result">Result.</param>
        /// <returns>True when parsing was successful.</returns>
        public static bool TryParseDouble(this string text, out double? result)
        {
            if (double.TryParse(text, NumberStyles.Number, CultureInfo.CurrentCulture, out var number))
            {
                result = number;
                return true;
            }
            result = null;
            return string.IsNullOrEmpty(text);
        }
    }
}