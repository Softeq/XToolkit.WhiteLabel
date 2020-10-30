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
    public class ObservableKeyGroupsCollectionTestsInsertItems
    {
        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullListItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateNullItemsList(),
                    x => x.SelectKey(),
                    x => x.SelectValue(),
                    x => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_EmptyListItems_CollectionNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var size = collection.Select(x => (x.Key, x.Count()));

            collection.InsertItems(
                     CollectionHelper.CreateEmptyItemsList(),
                     x => x.SelectKey(),
                     x => x.SelectValue(),
                     x => x.SelectIndex());

            foreach (var item in collection)
            {
                Assert.True(item.Count() == size.FirstOrDefault(x => x.Key == item.Key).Item2);
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullItem_NullReferenceException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<NullReferenceException>(
               () => collection.InsertItems(
                   CollectionHelper.CreateFillItemsListWithNull(),
                   x => x.SelectKey(),
                   x => x.SelectValue(),
                   x => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.InsertItemsListItemsWithExistKeysTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_ListItemsWithExistKeys_CollectionContainsNewItems(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.InsertItems(items, x => x.SelectKey(), x => x.SelectValue(), x => x.SelectIndex());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_ListItemsWithNewKeys_KeyNotFoundException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<KeyNotFoundException>(() => collection.InsertItems(
                     CollectionHelper.CreateFillItemsListWithNewKeys(),
                     x => x.SelectKey(),
                     x => x.SelectValue(),
                     x => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullKeySelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    null!,
                    x => x.SelectValue(),
                    x => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullValueSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    x => x.SelectKey(),
                    null!,
                    x => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_NullIndexSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateFillItemsListWithNewKeys(),
                    x => x.SelectKey(),
                    x => x.SelectValue(),
                    null!));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_SelectNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.InsertItems(
                    CollectionHelper.CreateFillItemsListWithExistKeys(),
                    x => null,
                    x => x.SelectValue(),
                    x => x.SelectIndex()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.InsertItemsIndexOutOfBoundsTestData), MemberType = typeof(CollectionHelper))]
        public void InsertItems_IndexOutOfBounds_ArgumentOutOfRangeException(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            int index)
        {
            Assert.Throws<ArgumentOutOfRangeException>(
                () => collection.InsertItems(items, x => x.SelectKey(), x => x.SelectValue(), x => index));
        }
    }
}
