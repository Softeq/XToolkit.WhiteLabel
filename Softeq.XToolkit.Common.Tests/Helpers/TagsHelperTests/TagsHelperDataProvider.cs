// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Text;
using Softeq.XToolkit.Common.Helpers;

namespace Softeq.XToolkit.Common.Tests.Helpers.TagsHelperTests
{
    internal static class TagsHelperDataProvider
    {
        public static string InputWithMultipleTags = "#tag1 and #tag2#tag3 and #tag4_6_7 and #tag5";

        public static TextRange[] TagsTextRangeArray = new TextRange[]
            {
                new TextRange(0, 5),
                new TextRange(10, 5),
                new TextRange(15, 5),
                new TextRange(25, 9),
                new TextRange(39, 5)
            };

        public static string[] TagsContentArray = new string[]
            {
                "tag1",
                "tag2",
                "tag3",
                "tag4_6_7",
                "tag5"
            };

        public static string GetResult(this TextRange[] textRangeArray)
        {
            var sb = new StringBuilder();
            sb.AppendJoin<TextRange>(",", textRangeArray);
            return sb.ToString();
        }

        public static string GetResult(this string[] tagsArray)
        {
            var sb = new StringBuilder();
            sb.AppendJoin(",", tagsArray);
            return sb.ToString();
        }
    }
}
