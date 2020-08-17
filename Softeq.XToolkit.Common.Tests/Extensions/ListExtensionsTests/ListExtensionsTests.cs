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
            var list = new List<int>();

            var result = list.AddItem(1);

            Assert.Same(list, result);
            Assert.Single(result);
        }

        [Fact]
        public void AddItem_NotEmptyList_ReturnsListWithAddedItem()
        {
            const int Expected = 4;
            var list = new List<int> { 1, 2, 3 };

            var result = list.AddItem(Expected);

            Assert.Same(list, result);
            Assert.Equal(Expected, result.Last());
        }

        [Fact]
        public void AddItem_EmptyListFilledFluently_ReturnsListWithAddedItems()
        {
            var list = new List<int>();

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
            var list = new List<int>();
            var range = new List<int>() { 1, 2, 3, 4 };

            list.AddRange(range);

            Assert.Equal(range.Count, list.Count);
            for (var i = 0; i < list.Count; i++)
            {
                Assert.Equal(range[i], list[i]);
            }
        }

        [Fact]
        public void AddRange_NotEmptyList_AddsItemsToList()
        {
            var list = new List<int> { 1, 2, 3 };
            var initialCount = list.Count;
            var range = new List<int>() { 1, 2, 3, 4 };

            list.AddRange(range);

            Assert.Equal(initialCount + range.Count, list.Count);
            for (var i = initialCount; i < list.Count; i++)
            {
                Assert.Equal(range[i - initialCount], list[i]);
            }
        }

        [Fact]
        public void AddRange_NotEmptyListWithEmptyRange_DoesNotChangeList()
        {
            var list = new List<int> { 1, 2, 3 };
            var initialCount = list.Count;
            var range = new List<int>();

            list.AddRange(range);

            Assert.Equal(initialCount, list.Count);
        }

        [Fact]
        public void InsertRange_EmptyListWithZeroIndex_InsertsItemsIntoList()
        {
            const int Index = 0;
            var list = new List<int>();
            var range = new List<int>() { 1, 2, 3, 4 };

            list.InsertRange(Index, range);

            Assert.Equal(range.Count, list.Count);
            for (var i = 0; i < list.Count; i++)
            {
                Assert.Equal(range[i], list[i]);
            }
        }

        [Fact]
        public void InsertRange_EmptyListWithIndexOutOfBounds_ThrowsException()
        {
            const int Index = 1;
            var list = new List<int>();
            var range = new List<int>() { 1, 2, 3, 4 };

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
            var list = new List<int> { 1, 2, 3 };
            var initialCount = list.Count;
            var range = new List<int>() { 1, 2, 3, 4 };

            list.InsertRange(index, range);

            Assert.Equal(initialCount + range.Count, list.Count);
            for (var i = index; i < index + range.Count; i++)
            {
                Assert.Equal(range[i - index], list[i]);
            }
        }

        [Fact]
        public void InsertRange_NotEmptyListWithEmptyRange_DoesNotChangeList()
        {
            const int Index = 1;
            var list = new List<int> { 1, 2, 3 };
            var initialCount = list.Count;
            var range = new List<int>();

            list.InsertRange(Index, range);

            Assert.Equal(initialCount, list.Count);
        }
    }
}
