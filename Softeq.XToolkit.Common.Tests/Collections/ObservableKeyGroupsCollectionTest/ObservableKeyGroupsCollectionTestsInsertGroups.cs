// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Collections;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class ObservableKeyGroupsCollectionTestsInsertGroups
    {
        [Fact]
        public void InsertGroups_KeysOnlyForbidEmptyGroupsNegativeIndex_ArgumentOutOfRangeException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentOutOfRangeException>(
                () => collection.InsertGroups(-1, CollectionHelper.KeysNotContained));
        }

        [Fact]
        public void InsertGroups_KeysOnlyForbidEmptyGroupsOverSizedIndex_ArgumentOutOfRangeException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentOutOfRangeException>(
                () => collection.InsertGroups(collection.Keys.Count + 1, CollectionHelper.KeysNotContained));
        }

        [Fact]
        public void InsertGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(
                () => collection.InsertGroups(0, CollectionHelper.KeysNotContained));
        }

        [Fact]
        public void InsertGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertGroups(0, CollectionHelper.KeysNull));
        }

        [Fact]
        public void InsertGroups_KeysOnlyAllowEmptyGroupEmptyListOfKeys_CollectionSizeNotChanged()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysEmpty;
            var expected = collection.Count + keys.Count;

            collection.InsertGroups(0, keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void InsertGroups_KeysOnlyAllowEmptyGroupNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertGroups(0, CollectionHelper.KeysOneNull));
        }

        [Fact]
        public void InsertGroups_KeysOnlyAllowEmptyGroupDuplicateKey_ArgumentException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentException>(
                () => collection.InsertGroups(0, CollectionHelper.KeysOneFill));
        }

        [Fact]
        public void InsertGroups_KeysOnlyAllowEmptyGroupUniqueKey_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var keys = CollectionHelper.KeysNotContained;
            var expected = collection.Count + keys.Count;

            collection.InsertGroups(0, keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNegativeIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => collection.InsertGroups(-1, CollectionHelper.PairNotContainedKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsOverSizedIndex_ArgumentOutOfRangeException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => collection.InsertGroups(collection.Keys.Count + 1, CollectionHelper.PairNotContainedKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertGroups(0, CollectionHelper.PairsNull));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsEmptyListOfPairs_CollectionSizeNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedCount = collection.Count;

            collection.InsertGroups(0, CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNullKeyWithItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNullKeyWithEmptyItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithEmptyItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsNullKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertGroups(0, CollectionHelper.PairNullKeyWithNullItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsUniqueKeysWithItems_CollectionSizeIncreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairNotContainedKeyWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.InsertGroups(0, pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsUniqueKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertGroups(0, CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact]
        public void InsertGroups_WithItemsAllowEmptyGroupsUniqueKeysWithEmptyItems_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            var pair = CollectionHelper.PairNotContainedKeyWithEmptyItem;
            var expectedCount = collection.Count + pair.Count;

            collection.InsertGroups(0, pair);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void InsertGroups_WithItemsForbidEmptyGroupsUniqueKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(
                () => collection.InsertGroups(0, CollectionHelper.PairWithKeyWithEmptyItem));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsDuplicateKeyWithItems_ArgumentException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentException>(
                () => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertGroups_WithItemsDuplicateKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithNullItems));
        }

        [Fact]
        public void InsertGroups_WithItemsForbidEmptyGroupsDuplicateKeysWithEmptyItems_ArgumentException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithoutEmpty();
            Assert.Throws<InvalidOperationException>(
                () => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithEmptyItem));
        }

        [Fact]
        public void InsertGroups_WithItemsAllowEmptyGroupsDuplicateKeysWithEmptyItems_ArgumentException()
        {
            var collection = CollectionHelper.CreateFilledGroupsWithEmpty();
            Assert.Throws<ArgumentException>(
                () => collection.InsertGroups(0, CollectionHelper.PairDuplicateKeyWithEmptyItem));
        }
    }
}
