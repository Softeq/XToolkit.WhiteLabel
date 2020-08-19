// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Extensions.ListExtensionsTests
{
    public class ListExtensionsTests
    {
        [Fact]
        public void AddItem_EmptyList_ReturnsListWithAddedItem()
        {
            IList<int> list = new List<int>();

            var result = list.AddItem(1);

            Assert.Same(list, result);
            Assert.Single(result);
        }

        [Fact]
        public void AddItem_NotEmptyList_ReturnsListWithAddedItem()
        {
            const int Expected = 4;
            IList<int> list = new List<int> { 1, 2, 3 };

            var result = list.AddItem(Expected);

            Assert.Same(list, result);
            Assert.Equal(Expected, result.Last());
        }

        [Fact]
        public void AddItem_EmptyListFilledFluently_ReturnsListWithAddedItems()
        {
            IList<int> list = new List<int>();

            var result = list
                .AddItem(1)
                .AddItem(2)
                .AddItem(3);

            Assert.Same(list, result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public void AddRange_EmptyList_AddsItemsToList()
        {
            IList<int> list = new List<int>();
            IEnumerable<int> range = new List<int>() { 1, 2, 3, 4 };

            list.AddRange(range);

            Assert.Equal(range.Count(), list.Count);
            var rangeArray = range.ToArray();
            for (var i = 0; i < list.Count; i++)
            {
                Assert.Equal(rangeArray[i], list[i]);
            }
        }

        [Fact]
        public void AddRange_NotEmptyList_AddsItemsToList()
        {
            IList<int> list = new List<int> { 1, 2, 3 };
            var initialCount = list.Count;
            IEnumerable<int> range = new List<int>() { 1, 2, 3, 4 };

            list.AddRange(range);

            Assert.Equal(initialCount + range.Count(), list.Count);
            var rangeArray = range.ToArray();
            for (var i = initialCount; i < list.Count; i++)
            {
                Assert.Equal(rangeArray[i - initialCount], list[i]);
            }
        }

        [Fact]
        public void AddRange_NotEmptyListWithEmptyRange_DoesNotChangeList()
        {
            IList<int> list = new List<int> { 1, 2, 3 };
            var initialCount = list.Count;
            IEnumerable<int> range = Enumerable.Empty<int>();

            list.AddRange(range);

            Assert.Equal(initialCount, list.Count);
        }

        [Fact]
        public void InsertRange_EmptyListWithZeroIndex_InsertsItemsIntoList()
        {
            const int Index = 0;
            IList<int> list = new List<int>();
            IEnumerable<int> range = new List<int>() { 1, 2, 3, 4 };

            list.InsertRange(Index, range);

            Assert.Equal(range.Count(), list.Count);
            var rangeArray = range.ToArray();
            for (var i = 0; i < list.Count; i++)
            {
                Assert.Equal(rangeArray[i], list[i]);
            }
        }

        [Fact]
        public void InsertRange_EmptyListWithIndexOutOfBounds_ThrowsException()
        {
            const int Index = 1;
            IList<int> list = new List<int>();
            IEnumerable<int> range = new List<int>() { 1, 2, 3, 4 };

            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                list.InsertRange(Index, range);
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void InsertRange_NotEmptyList_InsertsItemsIntoList(int index)
        {
            IList<int> list = new List<int> { 1, 2, 3 };
            var initialCount = list.Count;
            IEnumerable<int> range = new List<int>() { 1, 2, 3, 4 };

            list.InsertRange(index, range);

            Assert.Equal(initialCount + range.Count(), list.Count);
            var rangeArray = range.ToArray();
            for (var i = index; i < index + range.Count(); i++)
            {
                Assert.Equal(rangeArray[i - index], list[i]);
            }
        }

        [Fact]
        public void InsertRange_NotEmptyListWithEmptyRange_DoesNotChangeList()
        {
            const int Index = 1;
            IList<int> list = new List<int> { 1, 2, 3 };
            var initialCount = list.Count;
            IEnumerable<int> range = Enumerable.Empty<int>();

            list.InsertRange(Index, range);

            Assert.Equal(initialCount, list.Count);
        }
    }
}
