// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Tests.ObservableObjectTests.Utils;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableItemContentRangeCollectionTests
{
    public class ObservableItemContentRangeCollectionTests
    {
        private const string TestPropertyValue = "Property value";
        private const string TestPropertyValue2 = "Property value 2";

        [Theory]
        [MemberData(
            nameof(ObservableItemContentRangeCollectionDataProvider.CollectionData),
            MemberType = typeof(ObservableItemContentRangeCollectionDataProvider))]
        public void ObservableItemContentRangeCollection_WhenCreatedWithItems_AddsAllItems(
            IEnumerable<ObservableObject> items)
        {
            var collection = new ObservableItemContentRangeCollection<ObservableObject>(items);

            Assert.IsAssignableFrom<ObservableCollection<ObservableObject>>(collection);
            Assert.Equal(items.Count(), collection.Count);
            Assert.Equal(collection, items);
        }

        [Fact]
        public void ObservableItemContentRangeCollection_WhenCreatedWithEmptyItems_CreatesEmptyCollection()
        {
            var collection = new ObservableItemContentRangeCollection<ObservableObject>(new List<ObservableObject>());

            Assert.IsAssignableFrom<ObservableCollection<ObservableObject>>(collection);
            Assert.Empty(collection);
        }

        [Fact]
        public void ObservableItemContentRangeCollection_WhenCreatedWithNullItems_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => new ObservableItemContentRangeCollection<ObservableObject>(null!));
        }

        [Fact]
        public void ObservableItemContentRangeCollection_WhenCreatedWithoutItems_CreatesEmptyCollection()
        {
            var collection = new ObservableItemContentRangeCollection<ObservableObject>();

            Assert.IsAssignableFrom<ObservableCollection<ObservableObject>>(collection);
            Assert.Empty(collection);
        }

        [Fact]
        public void ObservableItemContentRangeCollection_WhenCreatedWithItems_NotifiesForInitialItems()
        {
            var item = new TestObservableObject();
            var items = new List<TestObservableObject>() { item, new TestObservableObject(), new TestObservableObject() };
            var collection = new ObservableItemContentRangeCollection<TestObservableObject>(items);
            var itemIndex = collection.IndexOf(item);

            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
                    handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                    handler => { },
                    () => item.TestProperty = TestPropertyValue);

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Replace, raisedEvent.Arguments.Action);
            Assert.Equal(itemIndex, raisedEvent.Arguments.NewStartingIndex);
            Assert.Equal(itemIndex, raisedEvent.Arguments.OldStartingIndex);
        }

        [Fact]
        public void ObservableItemContentRangeCollection_WhenCreatedWithItems_NotifiesForAddedItems()
        {
            var item = new TestObservableObject();
            var items = new List<TestObservableObject>() { item, new TestObservableObject(), new TestObservableObject() };
            var collection = new ObservableItemContentRangeCollection<TestObservableObject>(items);
            var addedItem = new TestObservableObject();
            collection.Add(addedItem);

            var itemIndex = collection.IndexOf(addedItem);

            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
                    handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                    handler => { },
                    () => addedItem.TestProperty = TestPropertyValue);

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Replace, raisedEvent.Arguments.Action);
            Assert.Equal(itemIndex, raisedEvent.Arguments.NewStartingIndex);
            Assert.Equal(itemIndex, raisedEvent.Arguments.OldStartingIndex);
        }

        [Fact]
        public void ObservableItemContentRangeCollection_WhenCreatedWithoutItems_NotifiesForAddedItems()
        {
            var item = new TestObservableObject();
            var collection = new ObservableItemContentRangeCollection<TestObservableObject>();
            collection.Add(item);
            var itemIndex = collection.IndexOf(item);

            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
                    handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                    handler => { },
                    () => item.TestProperty = TestPropertyValue);

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Replace, raisedEvent.Arguments.Action);
            Assert.Equal(itemIndex, raisedEvent.Arguments.NewStartingIndex);
            Assert.Equal(itemIndex, raisedEvent.Arguments.OldStartingIndex);
        }

        [Fact]
        public void ObservableItemContentRangeCollection_WhenCreatedWithItems_DoesNotNotifyForRemovedItems()
        {
            var item = new TestObservableObject();
            var items = new List<TestObservableObject>() { item, new TestObservableObject(), new TestObservableObject() };
            var collection = new ObservableItemContentRangeCollection<TestObservableObject>(items);

            collection.Remove(item);

            CustomAsserts.Assert_NotRaises<NotifyCollectionChangedEventArgs>(
                   handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                   handler => { },
                   () => item.TestProperty = TestPropertyValue2);
        }

        [Fact]
        public void ObservableItemContentRangeCollection_WhenCreatedWithoutItems_DoesNotNotifyForRemovedItems()
        {
            var item = new TestObservableObject();
            var collection = new ObservableItemContentRangeCollection<TestObservableObject>();

            CustomAsserts.Assert_NotRaises<NotifyCollectionChangedEventArgs>(
                    handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                    handler => { },
                    () => item.TestProperty = TestPropertyValue);

            collection.Add(item);
            collection.Remove(item);

            CustomAsserts.Assert_NotRaises<NotifyCollectionChangedEventArgs>(
                    handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                    handler => { },
                    () => item.TestProperty = TestPropertyValue2);
        }
    }
}
