﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Models;

namespace Softeq.XToolkit.Common.Helpers
{
    public static class TagsHelper
    {
        public const char TagStartSymbol = '#';
        public const char Underscore = '_';

        /// <summary>
        /// Find tags in specified string.
        /// </summary>
        /// <returns>Array of finded tags.</returns>
        /// <param name="input">A string containing tags.</param>
        public static string[] ExtractTags(string input)
        {
            var ranges = ExtractTagsRanges(input);
            var result = new List<string>();
            foreach (var range in ranges)
            {
                result.Add(input.Substring(range.Position + 1, range.Length - 1));
            }

            return result.ToArray();
        }

        /// <summary>
        /// Find positions of tags in specified string.
        /// </summary>
        /// <returns>The tags positions.</returns>
        /// <param name="input">A string containing tags.</param>
        public static TextRange[] ExtractTagsRanges(string input)
        {
            var result = new List<TextRange>();
            TextRange range = null;
            do
            {
                if (range != null)
                {
                    result.Add(range);
                }

                var startIndex = range != null ? range.Position + range.Length : 0;
                range = ExtractFirstTagRange(input, startIndex);
            } while (range != null);

            return result.ToArray();
        }

        private static TextRange ExtractFirstTagRange(string input, int startIndex)
        {
            if (startIndex >= input.Length)
            {
                return null;
            }

            var tagSymbolIndex = input.IndexOf(TagStartSymbol, startIndex);
            if (tagSymbolIndex < 0)
            {
                return null;
            }

            var tagEndIndex = tagSymbolIndex + 1;
            while (tagEndIndex < input.Length && IsSymbolValidForTag(input[tagEndIndex]))
            {
                tagEndIndex++;
            }

            var length = tagEndIndex - tagSymbolIndex;
            if (length <= 1)
            {
                return ExtractFirstTagRange(input, tagEndIndex);
            }

            return new TextRange(tagSymbolIndex, length);
        }

        private static bool IsSymbolValidForTag(char symbol)
        {
            return char.IsLetterOrDigit(symbol) || symbol == Underscore;
        }
    }
}