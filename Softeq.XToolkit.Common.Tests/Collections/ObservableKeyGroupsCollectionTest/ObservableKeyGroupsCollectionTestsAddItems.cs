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
    public class ObservableKeyGroupsCollectionTestsAddItems
    {
        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_NullListItems_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddItems(CollectionHelper.CreateNullItemsList(), (x) => x.SelectKey(), (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_EmptyListItems_CollectionNotChanged(ObservableKeyGroupsCollection<string, int> collection)
        {
            var size = collection.Select(x => (x.Key, x.Count()));

            collection.AddItems(CollectionHelper.CreateEmptyItemsList(), (x) => x.SelectKey(), (x) => x.SelectValue());

            foreach (var item in collection)
            {
                Assert.True(item.Count() == size.FirstOrDefault(x => x.Key == item.Key).Item2);
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_NullItem_NullReferenceException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<NullReferenceException>(
               () => collection.AddItems(CollectionHelper.CreateFillItemsListWithNull(), (x) => x.SelectKey(), (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.FillCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_ListCorrectItems_CollectionContainsNewItems(ObservableKeyGroupsCollection<string, int> collection)
        {
            var referenceValues = collection.ToDictionary(x => x.Key, x => x.ToList());
            var items = CollectionHelper.CreateFillItemsListWithNewKeys();

            collection.AddItems(items, (x) => x.SelectKey(), (x) => x.SelectValue());

            foreach (var item in items)
            {
                if (!referenceValues.ContainsKey(item.Key))
                {
                    referenceValues.Add(item.Key, new List<int>());
                }

                referenceValues[item.Key].Add(item.Value);
            }

            Assert.Equal(referenceValues.Count(), collection.Keys.Count);

            foreach (var item in collection)
            {
                Assert.True(item.SequenceEqual(referenceValues[item.Key]));
            }
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_NullKeySelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddItems(CollectionHelper.CreateFillItemsListWithNewKeys(), null, (x) => x.SelectValue()));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_NullValueSelector_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddItems(CollectionHelper.CreateFillItemsListWithNewKeys(), (x) => x.SelectKey(), null));
        }

        [Theory]
        [MemberData(nameof(CollectionHelper.EmptyCollectionOptionsTestData), MemberType = typeof(CollectionHelper))]
        public void AddItems_SelectNullKey_ArgumentNullException(ObservableKeyGroupsCollection<string, int> collection)
        {
            Assert.Throws<ArgumentNullException>(
                () => collection.AddItems(CollectionHelper.CreateFillItemsListWithNewKeys(), (x) => null, (x) => x.SelectValue()));
        }
    }
}
