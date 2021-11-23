// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Foundation;
using Softeq.XToolkit.Common.iOS.TextFilters;
using UIKit;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.TextFilters.ForbiddenCharsFilterTests
{
    public class ForbiddenCharsFilterTests
    {
        [Fact]
        public void Ctor_Empty_ReturnsITextFilter()
        {
            var obj = new ForbiddenCharsFilter();

            Assert.IsAssignableFrom<ITextFilter>(obj);
        }

        [Fact]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new ForbiddenCharsFilter(null!));
        }

        [Fact]
        public void Ctor_SingleChar_ReturnsITextFilter()
        {
            var obj = new ForbiddenCharsFilter('@');

            Assert.IsAssignableFrom<ITextFilter>(obj);
        }

        [Fact]
        public void Ctor_Chars_ReturnsITextFilter()
        {
            var obj = new ForbiddenCharsFilter('@', '+', '#');

            Assert.IsAssignableFrom<ITextFilter>(obj);
        }

        [Theory]
        [InlineData("", "", "", true)]
        [InlineData("", "old", "new", true)]
        [InlineData("#$", "old", "new#a", false)]
        [InlineData("#$@", "new", "@", false)]
        [InlineData("#$@", "old", "new", true)]
        public async Task ShouldChangeText_UITextFieldForbiddenChars_ReturnsExpectedResult(
            string forbiddenChars,
            string oldText,
            string newText,
            bool expectedResult)
        {
            var result = await Helpers.RunOnUIThreadAsync(() =>
            {
                var textField = new UITextField();
                var range = new NSRange(oldText.Length - 1, newText.Length);
                var chars = forbiddenChars.ToCharArray();
                var filter = new ForbiddenCharsFilter(chars);

                return filter.ShouldChangeText(textField, oldText, range, newText);
            });

            Assert.Equal(expectedResult, result);
        }

        [Theory]
        [InlineData(null, "old")]
        [InlineData(null, null)]
        public void ShouldChangeText_NullResponderAndOldText_ReturnsExpectedResult(UIResponder responder, string oldText)
        {
            var filter = new ForbiddenCharsFilter('%');

            var result = filter.ShouldChangeText(responder, oldText, new NSRange(1, 1), "new");

            Assert.True(result);
        }

        [Fact]
        public void ShouldChangeText_DefaultRange_ReturnsExpectedResult()
        {
            var filter = new ForbiddenCharsFilter('%');

            var result = filter.ShouldChangeText(null!, "old", default, "new");

            Assert.True(result);
        }
    }
}
