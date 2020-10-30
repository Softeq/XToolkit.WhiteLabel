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
    public class ObservableKeyGroupsCollectionTestsRemoveItems
    {
        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_NullListItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveItems(CollectionHelper.CreateNullItemsList(), x => x.SelectKey(), x => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_EmptyListItems_CollectionNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var size = collection.Select(x => (x.Key, x.Count()));

            collection.RemoveItems(CollectionHelper.CreateEmptyItemsList(), x => x.SelectKey(), x => x.SelectValue());

            foreach (var item in collection)
            {
                Assert.True(item.Count() == size.FirstOrDefault(x => x.Key == item.Key).Item2);
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_NullItem_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
               () => collection.RemoveItems(CollectionHelper.CreateFillItemsListWithNull(), x => x.SelectKey(), x => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsListItemsWithExistKeysTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_ListItemsWithExistKeys_ItemsSuccessRemoved(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.RemoveItems(items, x => x.SelectKey(), x => x.SelectValue());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_ItemsWithNotExistKeys_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(
                () => collection.RemoveItems(CollectionHelper.CreateFillItemsListWithNewKeys(), x => x.SelectKey(), x => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_ItemsWithNotExistValue_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(
                () => collection.RemoveItems(CollectionHelper.CreateFillItemsListWithExistKeys(), x => x.SelectKey(), x => int.MaxValue));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsAllItemsForExistKeyForbidEmptyTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_AllItemsForExistKeyForbidEmptyGroups_RemoveItemsRemoveEmptyGroups(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.RemoveItems(items, x => x.SelectKey(), x => x.SelectValue());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.RemoveItemsAllItemsForExistKeyAllowEmptyTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_AllItemsForExistKeyAllowEmptyGroups_RemoveItemsSaveEmptyGroups(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.RemoveItems(items, x => x.SelectKey(), x => x.SelectValue());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_NullKeySelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveItems(CollectionHelper.CreateFillItemsListWithExistKeys(), null!, x => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_NullValueSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveItems(CollectionHelper.CreateFillItemsListWithExistKeys(), x => x.SelectKey(), null!));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void RemoveItems_SelectNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.RemoveItems(CollectionHelper.CreateFillItemsListWithExistKeys(), x => null, x => x.SelectValue()));
        }
    }
}
