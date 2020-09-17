// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Text;
using Java.Lang;
using Softeq.XToolkit.Common.Droid.TextFilters;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.TextFilters.ForbiddenCharsFilterTests
{
    public class ForbiddenCharsInputFilterTests
    {
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("a")]
        [InlineData("aaa")]
        [InlineData("aA%@2-")]
        public void Ctor_WhenCalledWithAnyChars_CreatesFilter(string chars)
        {
            var obj = new ForbiddenCharsInputFilter(chars?.ToCharArray() ?? null!);

            Assert.IsAssignableFrom<IInputFilter>(obj);
        }

        [Theory]
        [PairwiseData]
        public void FilterFormatted_ForFilterWithNullChars_ReturnsNull(
            [CombinatorialValues(null, "", "b", "bbc", "+3b^ gb^")] string sourceStr,
            [CombinatorialValues(-1, 0, 1, 3)] int start,
            [CombinatorialValues(-1, 0, 1, 3)] int end,
            [CombinatorialValues(null, "", "a", "abc")] string? destStr,
            [CombinatorialValues(-1, 0, 1, 3)] int dstart,
            [CombinatorialValues(-1, 0, 1, 3)] int dend)
        {
            var source = new String(sourceStr);
            var dest = destStr == null ? null : new SpannableString(destStr);
            var filter = new ForbiddenCharsInputFilter(null!);

            var result = filter.FilterFormatted(source, start, end, dest, dstart, dend);

            Assert.Null(result);
        }

        [Theory]
        [PairwiseData]
        public void FilterFormatted_ForFilterWithNonNullChars_WhenCalledWithNullSource_ReturnsNull(
            [CombinatorialValues("", "a", "aaA%@2-")] string chars,
            [CombinatorialValues(-1, 0, 1, 3)] int start,
            [CombinatorialValues(-1, 0, 1, 3)] int end,
            [CombinatorialValues(null, "", "a", "abc")] string? destStr,
            [CombinatorialValues(-1, 0, 1, 3)] int dstart,
            [CombinatorialValues(-1, 0, 1, 3)] int dend)
        {
            var dest = destStr == null ? null : new SpannableString(destStr);
            var filter = new ForbiddenCharsInputFilter(chars.ToCharArray());

            var result = filter.FilterFormatted(null, start, end, dest, dstart, dend);

            Assert.Null(result);
        }

        [Theory]
        [PairwiseData]
        public void FilterFormatted_ForFilterWithNonNullChars_WhenCalledWithNonNullSource_IfSourceDoesNotContainForbiddenChars_ReturnsNull(
           [CombinatorialValues("", "a", "aaA%@2-")] string chars,
           [CombinatorialValues("", "b", "bbc", "+3b^ gb^")] string sourceStr,
           [CombinatorialValues(-1, 0, 1, 3)] int start,
           [CombinatorialValues(-1, 0, 1, 3)] int end,
           [CombinatorialValues(null, "", "a", "abc")] string? destStr,
           [CombinatorialValues(-1, 0, 1, 3)] int dstart,
           [CombinatorialValues(-1, 0, 1, 3)] int dend)
        {
            var source = new String(sourceStr);
            var dest = destStr == null ? null : new SpannableString(destStr);
            var filter = new ForbiddenCharsInputFilter(chars.ToCharArray());

            var result = filter.FilterFormatted(source, start, end, dest, dstart, dend);

            Assert.Null(result);
        }

        [Theory]
        [PairwiseData]
        public void FilterFormatted_ForFilterWithNonEmptyChars_WhenCalledWithNonNullSource_IfSourceContainForbiddenChars_ReturnsEmptyString(
           [CombinatorialValues("a", "aaabc", "%klp", "jd2ye")] string sourceStr,
           [CombinatorialValues(-1, 0, 1, 3)] int start,
           [CombinatorialValues(-1, 0, 1, 3)] int end,
           [CombinatorialValues(null, "", "a", "abc")] string? destStr,
           [CombinatorialValues(-1, 0, 1, 3)] int dstart,
           [CombinatorialValues(-1, 0, 1, 3)] int dend)
        {
            var source = new String(sourceStr);
            var dest = destStr == null ? null : new SpannableString(destStr);
            var filter = new ForbiddenCharsInputFilter("aaAA%@2@-".ToCharArray());

            var result = filter.FilterFormatted(source, start, end, dest, dstart, dend);

            Assert.Empty(result);
        }
    }
}
