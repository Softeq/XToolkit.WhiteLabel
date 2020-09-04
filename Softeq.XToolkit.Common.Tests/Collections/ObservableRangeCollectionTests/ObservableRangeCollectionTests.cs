// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Extensions;
using Xunit;

namespace Softeq.XToolkit.Common.Tests.Collections.ObservableRangeCollectionTests
{
    public class ObservableRangeCollectionTests
    {
        #region Initialize

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InitData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void ObservableRangeCollection_WhenCreatedWithItems_AddsAllItems(
            IEnumerable<int> items)
        {
            var collection = new ObservableRangeCollection<int>(items);

            Assert.IsAssignableFrom<ObservableCollection<int>>(collection);
            Assert.Equal(items.Count(), collection.Count);
            Assert.Equal(collection, items);
        }

        [Fact]
        public void ObservableRangeCollection_WhenCreatedWithEmptyItems_CreatesEmptyCollection()
        {
            var collection = new ObservableRangeCollection<int>(new List<int>());

            Assert.IsAssignableFrom<ObservableCollection<int>>(collection);
            Assert.Empty(collection);
        }

        [Fact]
        public void ObservableRangeCollection_WhenCreatedWithNullItems_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                var collection = new ObservableRangeCollection<int>(null);
            });
        }

        [Fact]
        public void ObservableRangeCollection_WhenCreatedWithoutItems_CreatesEmptyCollection()
        {
            var collection = new ObservableRangeCollection<int>();

            Assert.IsAssignableFrom<ObservableCollection<int>>(collection);
            Assert.Empty(collection);
        }

        #endregion

        #region AddRange

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ValidRangeWithWrongNotificationModesForAdding),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithValidRangeAndWrongNotificationMode_ThrowsException(
            IEnumerable<int> addItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>();
            Assert.Throws<ArgumentException>(() => collection.AddRange(addItems, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.NullRangeWithAnyNotificationMode),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithNullRange_ThrowsException(
            IEnumerable<int> addItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>();
            Assert.Throws<ArgumentNullException>(() => collection.AddRange(addItems, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ValidInitilalCollectionWithValidRange),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithValidRangeAndResetMode_AddsItemsAndNotifies(
            ObservableRangeCollection<int> collection, IEnumerable<int> addItems)
        {
            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
                handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                handler => { },
                () => collection.AddRange(addItems, NotifyCollectionChangedAction.Reset));

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Reset, raisedEvent.Arguments.Action);
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ValidInitilalCollectionWithValidRange),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithValidRangeAndAddMode_AddsItemsAndNotifies(
            ObservableRangeCollection<int> collection, IEnumerable<int> addItems)
        {
            var initialCount = collection.Count;

            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
                handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                handler => { },
                () => collection.AddRange(addItems, NotifyCollectionChangedAction.Add));

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Add, raisedEvent.Arguments.Action);
            Assert.Null(raisedEvent.Arguments.OldItems);
            Assert.Equal(initialCount, raisedEvent.Arguments.NewStartingIndex);

            var newItems = raisedEvent.Arguments.NewItems.Cast<int>().ToList();
            Assert.Equal(addItems, newItems);
        }

        #endregion

        #region InsertRange

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.NullRangeWithAnyStartIndex),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRange_WhenCalledWithNullRange_ThrowsException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            int startIndex)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertRange(insertItems, startIndex));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ValidRangeWithInvalidStartIndex),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRange_WhenCalledWithValidRangeAndInvalidStartIndex_ThrowsException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            int startIndex)
        {
            Assert.Throws<IndexOutOfRangeException>(() => collection.InsertRange(insertItems, startIndex));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ValidRangeWithValidStartIndex),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRange_WhenCalledWithValidRangeAndValidStartIndex_InsertsItemsCorrectlyAndNotifies(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            int startIndex)
        {
            var items = new List<int>(collection);
            items.InsertRange(startIndex, insertItems);

            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
               handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
               handler => { },
               () => collection.InsertRange(insertItems, startIndex));

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Add, raisedEvent.Arguments.Action);
            Assert.Null(raisedEvent.Arguments.OldItems);
            Assert.Equal(startIndex, raisedEvent.Arguments.NewStartingIndex);

            var newItems = raisedEvent.Arguments.NewItems.Cast<int>().ToList();
            Assert.Equal(insertItems, newItems);
            Assert.Equal(items, collection);
        }

        #endregion

        #region Replace

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ReplaceData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void Replace_WhenCalled_ClearsCollectionAddsItemAndNotifies(
            ObservableRangeCollection<int> collection,
            int item)
        {
            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
              handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
              handler => { },
              () => collection.Replace(item));

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Reset, raisedEvent.Arguments.Action);
            Assert.Single(collection);
            Assert.Equal(item, collection[0]);
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.CollectionData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void ReplaceRange_WhenCalledWithNullRange_ThrowsException(
            ObservableRangeCollection<int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceRange(null));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ReplaceRangeData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void ReplaceRange_WhenCalledWithNotNulRange_ClearsCollectionAddsItemsAndNotifies(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> replaceItems)
        {
            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
              handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
              handler => { },
              () => collection.ReplaceRange(replaceItems));

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Reset, raisedEvent.Arguments.Action);
            Assert.Equal(replaceItems, collection);
        }

        #endregion

        #region RemoveRange

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ValidRangeWithWrongNotificationModesForRemoving),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void RemoveRange_WhenCalledWithValidRangeAndWrongNotificationMode_ThrowsException(
            IEnumerable<int> removeItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>();
            Assert.Throws<ArgumentException>(() => collection.RemoveRange(removeItems, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.NullRangeWithAnyNotificationMode),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void RemoveRange_WhenCalledWithNullRange_ThrowsException(
            IEnumerable<int> removeItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>();
            Assert.Throws<ArgumentNullException>(() => collection.RemoveRange(removeItems, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ValidInitilalCollectionWithValidRange),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void RemoveRange_WhenCalledWithValidRangeAndResetMode_RemovesItemsAndNotifies(
            ObservableRangeCollection<int> collection, IEnumerable<int> removeItems)
        {
            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
                handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                handler => { },
                () => collection.RemoveRange(removeItems, NotifyCollectionChangedAction.Reset));

            var items = new List<int>(collection);
            foreach (var item in removeItems)
            {
                items.Remove(item);
            }

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Reset, raisedEvent.Arguments.Action);
            Assert.Equal(items, collection);
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ValidInitilalCollectionWithValidRange),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void RemoveRange_WhenCalledWithValidRangeAndRemoveMode_RemovesItemsAndNotifies(
            ObservableRangeCollection<int> collection, IEnumerable<int> removeItems)
        {
            var items = new List<int>(collection);
            var actualRemovedItems = new List<int>();
            foreach (var item in removeItems)
            {
                if (items.Remove(item))
                {
                    actualRemovedItems.Add(item);
                }
            }

            var raisedEvent = Assert.Raises<NotifyCollectionChangedEventArgs>(
                handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
                handler => { },
                () => collection.RemoveRange(removeItems, NotifyCollectionChangedAction.Remove));

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Remove, raisedEvent.Arguments.Action);
            Assert.Null(raisedEvent.Arguments.NewItems);
            Assert.Equal(items, collection);

            var oldItems = raisedEvent.Arguments.OldItems.Cast<int>().ToList();
            Assert.Equal(actualRemovedItems, oldItems);
        }

        #endregion

        #region InsertRangeSorted

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedNullRangeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithNullRange_ThrowsException(
            ObservableRangeCollection<int> collection,
            Comparison<int> comparison,
            NotifyCollectionChangedAction notificationMode)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertRangeSorted(null, comparison, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedNullComparisonTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithValidRangeAndNullComparison_ThrowsException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            NotifyCollectionChangedAction notificationMode)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertRangeSorted(insertItems, null, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedWrongNotificationModeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithValidRangeAndValidComparisonAndInvalidNotificationMode_ThrowsException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            Comparison<int> comparison,
            NotifyCollectionChangedAction notificationMode)
        {
            Assert.Throws<ArgumentException>(() => collection.InsertRangeSorted(insertItems, comparison, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedEverythingValidTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithValidRangeAndValidComparisonAndResetNotificationMode_InsertsItemsCorrectlyAndNotifies(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            Comparison<int> comparison)
        {
            collection.Sort(comparison);
            var items = new List<int>(collection);
            items.AddRange(insertItems);
            items.Sort(comparison);

            var raisedEvent = Assert.RaisesAny<NotifyCollectionChangedEventArgs>(
               handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
               handler => { },
               () => collection.InsertRangeSorted(insertItems, comparison, NotifyCollectionChangedAction.Reset));

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Reset, raisedEvent.Arguments.Action);
            Assert.Equal(items, collection);
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedEverythingValidTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithValidRangeAndValidComparisonAndAddNotificationMode_InsertsItemsCorrectlyAndNotifies(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            Comparison<int> comparison)
        {
            collection.Sort(comparison);
            var items = new List<int>(collection);
            items.AddRange(insertItems);
            items.Sort(comparison);

            var raisedEvent = Assert.RaisesAny<NotifyCollectionChangedEventArgs>(
               handler => collection.CollectionChanged += (s, e) => handler.Invoke(s, e),
               handler => { },
               () => collection.InsertRangeSorted(insertItems, comparison, NotifyCollectionChangedAction.Add));

            Assert.NotNull(raisedEvent);
            Assert.Equal(NotifyCollectionChangedAction.Add, raisedEvent.Arguments.Action);
            Assert.Null(raisedEvent.Arguments.OldItems);

            var newItems = raisedEvent.Arguments.NewItems.Cast<int>().ToList();
            //Assert.Equal(insertItems, newItems);
            Assert.Equal(items, collection);
        }

        #endregion
    }
}
