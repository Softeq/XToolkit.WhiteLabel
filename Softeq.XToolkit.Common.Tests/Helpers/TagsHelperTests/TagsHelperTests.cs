// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Helpers;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Helpers.TagsHelperTests
{
    public class TagsHelperTests
    {
        [Fact]
        public void ExtractTagsRanges_NullInput_ThrowsArgumentNullException()
        {
            string input = null;

            Assert.Throws<ArgumentNullException>(() => TagsHelper.ExtractTagsRanges(input));
        }

        [Theory]
        [InlineData("")]
        [InlineData("Some text without tags")]
        [InlineData("Some text with empty # tag")]
        public void ExtractTagsRanges_InputWithoutTags_ReturnsEmptyArray(string input)
        {
            var result = TagsHelper.ExtractTagsRanges(input);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("#tag", 0, 4)]
        [InlineData("#tag#", 0, 4)]
        [InlineData("##tag", 1, 4)]
        [InlineData("#tag in the beginning of the text", 0, 4)]
        [InlineData("Some text containing a #tag_123 with digits and underscore", 23, 8)]
        [InlineData("Some text ending with a #tag", 24, 4)]
        [InlineData("#tag& ending with some symbol", 0, 4)] // TODO: do we need to check all possible ending symbols
        public void ExtractTagsRanges_InputWithOneTag_ReturnsTagRange(string input, int tagPosition, int tagLength)
        {
            var textRange = new TextRange(tagPosition, tagLength);

            var result = TagsHelper.ExtractTagsRanges(input);

            Assert.Equal(1, result.Length);
            Assert.Equal(textRange.Position, result[0].Position);
            Assert.Equal(textRange.Length, result[0].Length);
        }

        [Theory]
        [MemberData(nameof(TagsHelperDataProvider.MultipleTagsTextRangeData), MemberType = typeof(TagsHelperDataProvider))]
        public void ExtractTagsRanges_InputWithMultipleTags_ReturnsCorrectTagRange(string input, string textRangesResult)
        {
            var result = TagsHelper.ExtractTagsRanges(input);

            Assert.Equal(textRangesResult, result.GetResult());
        }

        [Fact]
        public void ExtractTags_NullInput_ThrowsArgumentNullException()
        {
            string input = null;

            Assert.Throws<ArgumentNullException>(() => TagsHelper.ExtractTags(input));
        }

        [Theory]
        [InlineData("")]
        [InlineData("Some text without tags")]
        [InlineData("Some text with empty # tag")]
        public void ExtractTags_InputWithoutTags_ReturnsEmptyArray(string input)
        {
            var result = TagsHelper.ExtractTags(input);

            Assert.NotNull(result);
            Assert.Empty(result);
        }

        [Theory]
        [InlineData("#tag", "tag")]
        [InlineData("#tag#", "tag")]
        [InlineData("##tag", "tag")]
        [InlineData("#tag in the beginning of the text", "tag")]
        [InlineData("Some text containing a #tag_123 with digits and underscore", "tag_123")]
        [InlineData("Some text ending with a #tag", "tag")]
        [InlineData("#tag& ending with some symbol", "tag")] // TODO: do we need to check all possible ending symbols
        public void ExtractTags_InputWithOneTag_ReturnsTagRange(string input, string tag)
        {
            var result = TagsHelper.ExtractTags(input);

            Assert.Equal(1, result.Length);
            Assert.Equal(tag, result[0]);
        }

        [Theory]
        [MemberData(nameof(TagsHelperDataProvider.MultipleTagsContentData), MemberType = typeof(TagsHelperDataProvider))]
        public void ExtractTags_InputWithMultipleTags_ReturnsCorrectTagRange(string input, string[] tagsContent)
        {
            var stringResult = tagsContent.GetResult();

            var result = TagsHelper.ExtractTags(input);

            Assert.Equal(tagsContent.Length, result.Length);
            Assert.Equal(stringResult, result.GetResult());
        }
    }
}
