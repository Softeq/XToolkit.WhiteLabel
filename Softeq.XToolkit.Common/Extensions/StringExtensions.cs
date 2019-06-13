// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Softeq.XToolkit.Common.Extensions
{
    public static class StringExtensions
    {
        private const string LinkPattern = @"[a-z]+://([\w_-]+(?:(?:\.[\w_-]+)+))([\w.,@?^=%&:/~+#-]*[\w@?^=%&/~+#-])?";

        /// <summary>
        ///     Finds links in specified string.
        /// </summary>
        /// <returns>Strings that match the pattern xxx://xxxxxx.</returns>
        /// <param name="self">Specified string.</param>
        public static IEnumerable<Capture> FindLinks(this string self)
        {
            foreach (Capture match in Regex.Matches(self, LinkPattern, RegexOptions.IgnoreCase))
            {
                yield return match;
            }
        }
    }
}