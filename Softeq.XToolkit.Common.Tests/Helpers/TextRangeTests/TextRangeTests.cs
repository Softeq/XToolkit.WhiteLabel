// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Helpers;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Helpers.TextRangeTests
{
    public class TextRangeTests
    {
        [Theory]
        [InlineData(0, 0)]
        [InlineData(0, 10)]
        [InlineData(5, 10)]
        [InlineData(10, 10)]
        [InlineData(10, 5)]
        [InlineData(10, 0)]
        public void Create_NonNegativeParams_SavesState(int position, int length)
        {
            var textRange = new TextRange(position, length);

            Assert.Equal(textRange.Position, position);
            Assert.Equal(textRange.Length, length);
        }

        [Theory]
        [InlineData(-1, 0)]
        [InlineData(0, -1)]
        [InlineData(-1, -1)]
        public void Create_NegativeParams_ThrowsArgumentException(int position, int length)
        {
            Assert.Throws<ArgumentException>(() => new TextRange(position, length));
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", null)]
        [InlineData(null, null)]
        public void BuildNewString_NullParams_ThrowsArgumentNullException(string fullString, string newString)
        {
            var textRange = TextRangeDataProvider.CreateFisrtSymbolTextRange();

            Assert.Throws<ArgumentNullException>(() => textRange.BuildNewString(fullString, newString));
        }

        [Theory]
        [MemberData(nameof(TextRangeDataProvider.OutOfRangeBuildStringData), MemberType = typeof(TextRangeDataProvider))]
        public void BuildNewString_PositionOutOfRange_ThrowsArgumentOutOfRangeException(string fullString, string newString,
            int textRangePosition, int textRangeLength)
        {
            var textRange = new TextRange(textRangePosition, textRangeLength);

            Assert.Throws<ArgumentOutOfRangeException>(() => textRange.BuildNewString(fullString, newString));
        }

        [Theory]
        [MemberData(nameof(TextRangeDataProvider.ValidTextRangeBuildStringData), MemberType = typeof(TextRangeDataProvider))]
        public void BuildNewString_ValidParams_ConstructsCorrectString(string fullString, string newString,
            int textRangePosition, int textRangeLength, string resultString)
        {
            var textRange = new TextRange(textRangePosition, textRangeLength);

            var result = textRange.BuildNewString(fullString, newString);

            Assert.Equal(resultString, result);
        }
    }
}
