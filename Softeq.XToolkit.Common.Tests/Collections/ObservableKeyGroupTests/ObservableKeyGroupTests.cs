// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common.Collections;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupTests
{
    public class ObservableKeyGroupTests
    {
        [Theory]
        [MemberData(
            nameof(ObservableKeyGroupDataProvider.KeysData),
            MemberType = typeof(ObservableKeyGroupDataProvider))]
        public void ObservableKeyGroup_WhenCreatedWithKey_CreatesEmptyGroup(string key)
        {
            var group = new ObservableKeyGroup<string, int>(key);

            Assert.IsAssignableFrom<ObservableRangeCollection<int>>(group);
            Assert.Empty(group);
            Assert.Equal(key, group.Key);
        }

        [Theory]
        [MemberData(
            nameof(ObservableKeyGroupDataProvider.KeysData),
            MemberType = typeof(ObservableKeyGroupDataProvider))]
        public void ObservableKeyGroup_WhenCreatedWithKeyAndEmptyItems_CreatesEmptyGroup(string key)
        {
            var group = new ObservableKeyGroup<string, int>(key, new List<int>());

            Assert.IsAssignableFrom<ObservableRangeCollection<int>>(group);
            Assert.Empty(group);
            Assert.Equal(key, group.Key);
        }

        [Theory]
        [MemberData(
            nameof(ObservableKeyGroupDataProvider.KeysAndItemsData),
            MemberType = typeof(ObservableKeyGroupDataProvider))]
        public void ObservableKeyGroup_WhenCreatedWithKeyAndNonEmptyItems_CreatesGroupAndAddsAllItems(
            string key,
            IEnumerable<int> items)
        {
            var group = new ObservableKeyGroup<string, int>(key, items);

            Assert.IsAssignableFrom<ObservableRangeCollection<int>>(group);
            Assert.Equal(key, group.Key);
            Assert.Equal(items, group);
        }

        [Theory]
        [MemberData(
            nameof(ObservableKeyGroupDataProvider.KeysData),
            MemberType = typeof(ObservableKeyGroupDataProvider))]
        public void ObservableKeyGroup_WhenCreatedWithKeyAndNullItems_ThrowsCorrectException(string key)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var collection = new ObservableKeyGroup<string, int>(key, null);
            });
        }
    }
}
