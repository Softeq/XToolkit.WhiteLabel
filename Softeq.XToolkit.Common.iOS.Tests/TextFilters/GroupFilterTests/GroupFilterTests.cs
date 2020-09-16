// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.iOS.TextFilters;
using Xunit;

namespace Softeq.XToolkit.Common.iOS.Tests.TextFilters.GroupFilterTests
{
    public class GroupFilterTests
    {
        [Fact]
        public void Ctor_Empty_ReturnsITextFilter()
        {
            var obj = new GroupFilter();

            Assert.IsAssignableFrom<ITextFilter>(obj);
        }

        [Fact]
        public void Ctor_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new GroupFilter(null!);
            });
        }

        [Fact]
        public void Ctor_SingleFilter_ReturnsITextFilter()
        {
            var filter = new MockTextFilter(true);

            var obj = new GroupFilter(filter);

            Assert.IsAssignableFrom<ITextFilter>(obj);
        }

        [Fact]
        public void Ctor_Filters_ReturnsITextFilter()
        {
            var filter1 = new MockTextFilter(true);
            var filter2 = new MockTextFilter(true);

            var obj = new GroupFilter(filter1, filter2);

            Assert.IsAssignableFrom<ITextFilter>(obj);
        }

        [Theory]
        [InlineData(true, true, true)]
        [InlineData(true, false, false)]
        [InlineData(false, true, false)]
        [InlineData(false, false, false)]
        public void ShouldChangeText_FilterReturnsResult_ReturnsExpected(
            bool filter1Result,
            bool filter2Result,
            bool expectedResult)
        {
            var filter1 = new MockTextFilter(filter1Result);
            var filter2 = new MockTextFilter(filter2Result);
            var groupFilter = new GroupFilter(filter1, filter2);

            var result = groupFilter.ShouldChangeText(null!, string.Empty, default, string.Empty);

            Assert.Equal(expectedResult, result);
        }
    }
}
