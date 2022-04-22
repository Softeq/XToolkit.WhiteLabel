// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using Softeq.XToolkit.Common.Helpers;

namespace Softeq.XToolkit.Common.Tests.Helpers.TextRangeTests
{
    internal static class TextRangeDataProvider
    {
        public const string TestFullString = "Full string for test";

        public static IEnumerable<object[]> ValidTextRangeBuildStringData
        {
            get
            {
                // Replace in the middle with text
                yield return new object[] { TestFullString, "new", 5, 6, "Full new for test" };

                // Replace in the middle with empty string
                yield return new object[] { TestFullString, string.Empty, 5, 6, "Full  for test" };

                // Insert in the beginning (Length 0)
                yield return new object[] { TestFullString, "new", 0, 0, "newFull string for test" };

                // Insert in the middle (Length 0)
                yield return new object[] { TestFullString, "new", 5, 0, "Full newstring for test" };

                // Insert in the end (Length 0)
                yield return new object[] { TestFullString, "new", TestFullString.Length, 0, "Full string for testnew" };

                // Replace in empty string with text
                yield return new object[] { string.Empty, "new", 0, 0, "new" };

                // Replace in empty string with empty string
                yield return new object[] { string.Empty, string.Empty, 0, 0, string.Empty };
            }
        }

        public static IEnumerable<object[]> OutOfRangeBuildStringData
        {
            get
            {
                // Empty full string - non-zero length of range
                yield return new object[] { string.Empty, "new", 0, 1 };

                // Empty full string - non-zero position of range
                yield return new object[] { string.Empty, "new", 1, 0 };

                // Range starting index outside full string
                yield return new object[] { TestFullString, "new", TestFullString.Length + 1, 5 };

                // Range ending index outside full string
                yield return new object[] { TestFullString, "new", TestFullString.Length - 1, 5 };
            }
        }

        public static TextRange CreateFirstSymbolTextRange() => new TextRange(0, 1);
    }
}
