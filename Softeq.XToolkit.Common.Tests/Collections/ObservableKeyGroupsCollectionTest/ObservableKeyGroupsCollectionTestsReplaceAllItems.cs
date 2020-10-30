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
    public class ObservableKeyGroupsCollectionTestsReplaceAllItems
    {
        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllItems_NullListItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllItems(CollectionHelper.CreateNullItemsList(), x => x.SelectKey(), x => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllItems_EmptyListItems_CollectionNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var size = collection.Select(x => (x.Key, x.Count()));

            collection.ReplaceAllItems(CollectionHelper.CreateEmptyItemsList(), x => x.SelectKey(), x => x.SelectValue());

            foreach (var item in collection)
            {
                Assert.True(item.Count() == size.FirstOrDefault(x => x.Key == item.Key).Item2);
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllItems_NullItem_NullReferenceException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<NullReferenceException>(
               () => collection.ReplaceAllItems(CollectionHelper.CreateFillItemsListWithNull(), x => x.SelectKey(), x => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.ReplaceItemsListItemsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllItems_ListItems_CollectionContainsReplacedItems(
            ObservableKeyGroupsCollection<string, int> collection,
            List<TestItem<string, int>> items,
            ObservableKeyGroupsCollection<string, int> resultCollection)
        {
            collection.ReplaceAllItems(items, x => x.SelectKey(), x => x.SelectValue());

            Assert.Equal(resultCollection.Keys.Count, collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(resultCollection.First(x => item.Key == x.Key)));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllItems_NullKeySelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllItems(CollectionHelper.CreateFillItemsListWithNewKeys(), null!, x => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllItems_NullValueSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllItems(CollectionHelper.CreateFillItemsListWithNewKeys(), x => x.SelectKey(), null!));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void ReplaceAllItems_SelectNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.ReplaceAllItems(CollectionHelper.CreateFillItemsListWithExistKeys(), x => null, x => x.SelectValue()));
        }
    }
}
