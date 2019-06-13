// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
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
    }
}