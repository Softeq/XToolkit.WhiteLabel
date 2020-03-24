// Developed by Softeq Development Corporation
// http://www.softeq.com

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
        public void AddItem_EmptyListFilledFluent_ReturnsListWithAddedItems()
        {
            var list = new List<int>();

            var result = list
                .AddItem(1)
                .AddItem(2)
                .AddItem(3);

            Assert.Same(list, result);
            Assert.Equal(3, result.Count);
        }
    }
}
