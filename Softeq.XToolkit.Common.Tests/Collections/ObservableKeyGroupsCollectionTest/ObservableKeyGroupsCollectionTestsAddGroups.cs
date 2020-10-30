// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.Common.Collections;
using Xunit;

using CollectionHelper = Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest.ObservableKeyGroupsCollectionHelper;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableKeyGroupsCollectionTest
{
    public class ObservableKeyGroupsCollectionTestsAddGroups
    {
        [Fact]
        public void AddGroups_KeysOnlyForbidEmptyGroup_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            Assert.Throws<InvalidOperationException>(
                () => collection.AddGroups(CollectionHelper.KeysNull));
        }

        [Fact]
        public void AddGroups_KeysOnlyAllowEmptyGroupNullListOfKeys_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            Assert.Throws<ArgumentNullException>(
                () => collection.AddGroups(CollectionHelper.KeysNull));
        }

        [Fact]
        public void AddGroups_KeysOnlyAllowEmptyGroupsEmptyListOfKeys_CollectionSizeNotChanged()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var expected = collection.Keys.Count;

            collection.AddGroups(CollectionHelper.KeysEmpty);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void AddGroups_KeysOnlyAllowEmptyGroupsNullKey_ArgumentNullException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            Assert.Throws<ArgumentNullException>(
                () => collection.AddGroups(CollectionHelper.KeysOneFillOneNull));
        }

        [Fact]
        public void AddGroups_KeysOnlyAllowEmptyGroupsUniqueKeys_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var keys = CollectionHelper.KeysOneFillOneEmpty;
            var expected = collection.Keys.Count + keys.Count;

            collection.AddGroups(keys);

            Assert.Equal(expected, collection.Keys.Count);
        }

        [Fact]
        public void AddGroups_KeysOnlyAllowEmptyGroupsDuplicateKeys_ArgumentException()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            Assert.Throws<ArgumentException>(
                () => collection.AddGroups(CollectionHelper.KeysDuplicate));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsNullListOfPairs_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddGroups(CollectionHelper.PairsNull));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsEmptyListOfPairs_CollectionSizeNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var expectedCount = collection.Count;

            collection.AddGroups(CollectionHelper.PairsEmpty);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsUniqueKeysWithItems_CollectionSizeIncreased(ObservableKeyGroupsCollection<string, int> collection)
        {
            var pairs = CollectionHelper.PairsWithKeysWithItems;
            var expectedCount = collection.Count + pairs.Count;

            collection.AddGroups(pairs);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsUniqueKeysWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddGroups(CollectionHelper.PairWithKeyWithNullItems));
        }

        [Fact]
        public void AddGroups_WithItemsAllowEmptyGroupsUniqueKeysWithEmptyItems_CollectionSizeIncreased()
        {
            var collection = CollectionHelper.CreateWithEmptyGroups();
            var pair = CollectionHelper.PairWithKeyWithEmptyItem;
            var expectedCount = collection.Count + pair.Count;

            collection.AddGroups(pair);

            Assert.Equal(expectedCount, collection.Count);
        }

        [Fact]
        public void AddGroups_WithItemsForbidEmptyGroupsUniqueKeysWithEmptyItems_InvalidOperationException()
        {
            var collection = CollectionHelper.CreateWithoutEmptyGroups();
            Assert.Throws<InvalidOperationException>(
                () => collection.AddGroups(CollectionHelper.PairWithKeyWithEmptyItem));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.CollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsDuplicateKeyWithItems_ArgumentException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentException>(
                () => collection.AddGroups(CollectionHelper.PairDuplicateKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsNullKeyWithItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddGroups(CollectionHelper.PairNullKeyWithItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsNullKeyWithEmptyItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddGroups(CollectionHelper.PairNullKeyWithEmptyItems));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddGroups_WithItemsNullKeyWithNullItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddGroups(CollectionHelper.PairNullKeyWithNullItems));
        }
    }
}
