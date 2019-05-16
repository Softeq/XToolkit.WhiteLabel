// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Models;

namespace Softeq.XToolkit.Common.Helpers
{
    public static class TagsHelper
    {
        public const char TagStartSymbol = '#';
        public const char SpaceSymbol = ' ';
        public const char Underscore = '_';

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

        public static TextRange ExtractFirstTagRange(string input, int startIndex)
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

        public static bool IsSymbolValidForTag(char symbol)
        {
            return char.IsLetterOrDigit(symbol) || symbol == Underscore;
        }
    }
}