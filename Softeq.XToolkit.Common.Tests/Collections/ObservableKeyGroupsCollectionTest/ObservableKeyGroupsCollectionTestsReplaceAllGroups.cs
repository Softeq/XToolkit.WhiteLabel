// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class ObservableKeyGroupsCollectionTestsReplaceAllGroups
    {
        [Fact]
        public void ReplaceAllGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(
                () => collection.ReplaceAllGroups(CollectionHelper.KeysNotContained));
        }

        [Fact]
        public void ReplaceAllGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllGroups(CollectionHelper.KeysNull));
        }

        [Fact]
        public void ReplaceAllGroups_KeysOnlyAllowEmptyGroupEmptyListOfKeys_CollectionIsEmpty()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            collection.ReplaceAllGroups(CollectionHelper.KeysEmpty);

            Assert.Equal(0, collection.Keys.Count);
        }

        [Fact]
        public void ReplaceAllGroups_KeysOnlyAllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllGroups(CollectionHelper.KeysOneNull));
        }

        [Fact]
        public void ReplaceAllGroups_KeysOnlyAllowEmptyGroupCorrectKeys_CollectionContainsOnlyNewKeys()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysOneContainedOneNotContained;

            collection.ReplaceAllGroups(keys);

            foreach (var key in keys)
            {
                Assert.True(collection.Keys.Contains(key));
            }

            Assert.Equal(keys.Count, collection.Keys.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllGroups(CollectionHelper.PairsNull));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllGroups_WithItemsEmptyListOfPairs_CollectionIsEmpty(ObservableKeyGroupsCollection<string, int> collection)
        {
            collection.ReplaceAllGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(0, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceGroupsWithItemsNullKeyTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllGroups_WithItemsNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection, IList<KeyValuePair<string, IList<int>>> pairs)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllGroups(pairs));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllGroups_WithItemsCorrectKeysWithItems_CollectionContainsOnlyNewKeys(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;

            collection.ReplaceAllGroups(pairs);

            Assert.Equal(pairs.Count, collection.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(pairs.FirstOrDefault(x => x.Key == item.Key).Value));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllGroups_WithItemsCorrectKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllGroups(CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact]
        public void ReplaceAllGroups_WithItemsAllowEmptyGroupsCorrectKeysWithEmptyItems_CollectionContainsOnlyNewKeys()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var pairs = CollectionHelper.PairWithKeyWithEmptyItem;

            collection.ReplaceAllGroups(pairs);

            Assert.Equal(pairs.Count, collection.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(pairs.FirstOrDefault(x => x.Key == item.Key).Value));
            }
        }

        [Fact]
        public void ReplaceAllGroups_WithItemsForbidEmptyGroupsCorrectKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(
                () => collection.ReplaceAllGroups(CollectionHelper.PairWithKeyWithEmptyItem));
        }
    }
}
