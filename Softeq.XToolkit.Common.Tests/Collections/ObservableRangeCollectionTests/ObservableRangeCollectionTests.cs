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
        public void ObservableRangeCollection_WhenCreatedWithNullItems_ThrowsCorrectException()
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
        [InlineData(NotifyCollectionChangedAction.Add)]
        [InlineData(NotifyCollectionChangedAction.Move)]
        [InlineData(NotifyCollectionChangedAction.Remove)]
        [InlineData(NotifyCollectionChangedAction.Replace)]
        [InlineData(NotifyCollectionChangedAction.Reset)]
        public void AddRange_WhenCalledWithNullRange_ThrowsCorrectException(
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>();
            Assert.Throws<ArgumentNullException>(() => collection.AddRange(null, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.AddRangeValidRangeWithWrongModesTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithValidRangeAndWrongNotificationMode_ThrowsCorrectException(
            IEnumerable<int> addItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>();
            Assert.Throws<ArgumentException>(() => collection.AddRange(addItems, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.AddOrRemoveRangeValidInitilalCollectionWithValidRangeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithValidRangeAndResetMode_AddsItemsAndNotifies(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> addItems)
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
            nameof(ObservableRangeCollectionDataProvider.AddOrRemoveRangeValidInitilalCollectionWithValidRangeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithValidRangeAndAddMode_AddsItemsAndNotifies(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> addItems)
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
            nameof(ObservableRangeCollectionDataProvider.InsertRangeNullRangeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRange_WhenCalledWithNullRange_ThrowsCorrectException(
            ObservableRangeCollection<int> collection,
            int startIndex)
        {
            Assert.Throws<ArgumentNullException>(() => collection.InsertRange(null, startIndex));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeValidRangeWithInvalidStartIndexTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRange_WhenCalledWithValidRangeAndInvalidStartIndex_ThrowsCorrectException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            int startIndex)
        {
            Assert.Throws<IndexOutOfRangeException>(() => collection.InsertRange(insertItems, startIndex));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeValidRangeWithValidStartIndexTestData),
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
            nameof(ObservableRangeCollectionDataProvider.ReplaceTestData),
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
        public void ReplaceRange_WhenCalledWithNullRange_ThrowsCorrectException(
            ObservableRangeCollection<int> collection)
        {
            Assert.Throws<ArgumentNullException>(() => collection.ReplaceRange(null));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ReplaceRangeTestData),
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
            nameof(ObservableRangeCollectionDataProvider.RemoveRangeValidRangeWithWrongModesTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void RemoveRange_WhenCalledWithValidRangeAndWrongNotificationMode_ThrowsCorrectException(
            IEnumerable<int> removeItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>();
            Assert.Throws<ArgumentException>(() => collection.RemoveRange(removeItems, notificationMode));
        }

        [Theory]
        [InlineData(NotifyCollectionChangedAction.Add)]
        [InlineData(NotifyCollectionChangedAction.Move)]
        [InlineData(NotifyCollectionChangedAction.Remove)]
        [InlineData(NotifyCollectionChangedAction.Replace)]
        [InlineData(NotifyCollectionChangedAction.Reset)]
        public void RemoveRange_WhenCalledWithNullRange_ThrowsCorrectException(
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>();
            Assert.Throws<ArgumentNullException>(() => collection.RemoveRange(null, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.AddOrRemoveRangeValidInitilalCollectionWithValidRangeTestData),
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
            Assert.Equal(-1, raisedEvent.Arguments.OldStartingIndex);
            Assert.Equal(items, collection);
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.AddOrRemoveRangeValidInitilalCollectionWithValidRangeTestData),
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
            if (actualRemovedItems.Count == 0)
            {
                Assert.Equal(-1, raisedEvent.Arguments.OldStartingIndex);
            }
            else
            {
                Assert.True(raisedEvent.Arguments.OldStartingIndex > -1);
            }
        }

        #endregion

        #region InsertRangeSorted

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedNullRangeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithNullRange_ThrowsCorrectException(
            Comparison<int> comparison,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int> { 1, 2, 3 };

            Assert.Throws<ArgumentNullException>(() => collection.InsertRangeSorted(null, comparison, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedNullComparisonTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithValidRangeAndNullComparison_ThrowsCorrectException(
            IEnumerable<int> insertItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int> { 1, 2, 3 };

            Assert.Throws<ArgumentNullException>(() => collection.InsertRangeSorted(insertItems, null, notificationMode));
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedWrongModeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithValidRangeAndValidComparisonAndInvalidNotificationMode_ThrowsCorrectException(
            IEnumerable<int> insertItems,
            Comparison<int> comparison,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int> { 1, 2, 3 };

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

            Assert.Equal(items, collection);
        }

        #endregion

        #region CheckReentrancy_AddRange

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.AddRangeNonEmptyRangeWithValidModesTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithNonEmptyRangeAndValidNotificationMode_InForeach_ThrowsCorrectException(
            IEnumerable<int> addItems,
            NotifyCollectionChangedAction action)
        {
            var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.AddRange(addItems, action);
                }
            });
        }

        [Theory]
        [InlineData(NotifyCollectionChangedAction.Add)]
        [InlineData(NotifyCollectionChangedAction.Reset)]
        public void AddRange_WhenCalledWithEmptyRangeAndValidMode_InForeach_DoesNotThrowException(
            NotifyCollectionChangedAction action)
        {
            var items = new List<int> { 1, 2, 3 };
            var collection = new ObservableRangeCollection<int>(items);

            foreach (var item in collection)
            {
                collection.AddRange(new List<int>(), action);
            }

            Assert.Equal(items, collection);
        }

        [Theory]
        [InlineData(NotifyCollectionChangedAction.Add)]
        [InlineData(NotifyCollectionChangedAction.Move)]
        [InlineData(NotifyCollectionChangedAction.Remove)]
        [InlineData(NotifyCollectionChangedAction.Replace)]
        [InlineData(NotifyCollectionChangedAction.Reset)]
        public void AddRange_WhenCalledWithNullRange_InForeach_ThrowsCorrectException(
            NotifyCollectionChangedAction action)
        {
            var collection = new ObservableRangeCollection<int>() { 1, 2, 3 };
            Assert.Throws<ArgumentNullException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.AddRange(null, action);
                }
            });
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.AddRangeValidRangeWithWrongModesTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void AddRange_WhenCalledWithValidRangeAndWrongNotificationMode_InForeach_ThrowsCorrectException(
            IEnumerable<int> addItems,
            NotifyCollectionChangedAction action)
        {
            var collection = new ObservableRangeCollection<int> { 1, 2, 3 };
            Assert.Throws<ArgumentException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.AddRange(addItems, action);
                }
            });
        }

        #endregion

        #region CheckReentrancy_InsertRange

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeNonEmptyCollectionWithNonEmptyRangeWithValidStartIndexTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRange_WhenCalledWithNonEmptyRangeAndValidIndex_InForeach_ThrowsCorrectException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            int startIndex)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.InsertRange(insertItems, startIndex);
                }
            });
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void InsertRange_WhenCalledWithEmptyRangeAndValidIndex_InForeach_DoesNotThrowException(
            int startIndex)
        {
            var items = new List<int> { 1, 2, 3 };
            var collection = new ObservableRangeCollection<int>(items);

            foreach (var item in collection)
            {
                collection.InsertRange(new List<int>(), startIndex);
            }

            Assert.Equal(items, collection);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void InsertRange_WhenCalledWithNullRange_InForeach_ThrowsCorrectException(
            int startIndex)
        {
            var collection = new ObservableRangeCollection<int> { 1, 2, 3 };

            Assert.Throws<ArgumentNullException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.InsertRange(null, startIndex);
                }
            });
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeNonEmptyCollectionWithValidRangeWithInvalidStartIndexTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRange_WhenCalledWithValidRangeAndInvalidStartIndex_InForeach_ThrowsCorrectException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            int startIndex)
        {
            Assert.Throws<IndexOutOfRangeException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.InsertRange(insertItems, startIndex);
                }
            });
        }

        #endregion

        #region CheckReentrancy_Replace

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ReplaceNonEmptyCollectionTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void Replace_WhenCalled_InForeach_ThrowsCorrectException(
            ObservableRangeCollection<int> collection,
            int replaceItem)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.Replace(replaceItem);
                }
            });
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.NonEmptyCollectionData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void ReplaceRange_WhenCalledWithNullRange_InForeach_ThrowsCorrectException(
            ObservableRangeCollection<int> collection)
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.ReplaceRange(null);
                }
            });
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.ReplaceRangeNonEmptyCollectionTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void ReplaceRange_WhenCalledWithNotNulRange_InForeach_ThrowsCorrectException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> replaceItems)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.ReplaceRange(replaceItems);
                }
            });
        }

        #endregion

        #region CheckReentrancy_RemoveRange

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.RemoveRangeValidRangeWithWrongModesTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void RemoveRange_WhenCalledWithValidRangeAndWrongNotificationMode_InForeach_ThrowsCorrectException(
            IEnumerable<int> removeItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>() { 1, 2, 3 };

            Assert.Throws<ArgumentException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.RemoveRange(removeItems, notificationMode);
                }
            });
        }

        [Theory]
        [InlineData(NotifyCollectionChangedAction.Add)]
        [InlineData(NotifyCollectionChangedAction.Move)]
        [InlineData(NotifyCollectionChangedAction.Remove)]
        [InlineData(NotifyCollectionChangedAction.Replace)]
        [InlineData(NotifyCollectionChangedAction.Reset)]
        public void RemoveRange_WhenCalledWithNullRange_InForeach_ThrowsCorrectException(
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>() { 1, 2, 3 };
            Assert.Throws<ArgumentNullException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.RemoveRange(null, notificationMode);
                }
            });
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.RemoveRangeNonEmptyCollectionWithValidRangeAndValidModeWithNoItemsToRemoveTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void RemoveRange_WhenCalledWithValidRangeAndMode_NoItemsToRemove_InForeach_DoesNotThrowException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> removeItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var items = collection.ToList();

            foreach (var item in collection)
            {
                collection.RemoveRange(removeItems, notificationMode);
            }

            Assert.Equal(items, collection);
        }

        [Theory]
        [MemberData(
           nameof(ObservableRangeCollectionDataProvider.RemoveRangeNonEmptyCollectionWithValidRangeAndValidModeWithItemsToRemoveTestData),
           MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void RemoveRange_WhenCalledWithValidRangeAndMode_HasItemsToRemove_InForeach_ThrowsCorrectException(
           ObservableRangeCollection<int> collection,
           IEnumerable<int> removeItems,
           NotifyCollectionChangedAction notificationMode)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.RemoveRange(removeItems, notificationMode);
                }
            });
        }

        #endregion

        #region CheckReentrancy_InsertRangeSorted

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedNullRangeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithNullRange_InForeach_ThrowsCorrectException(
            Comparison<int> comparison,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>() { 1, 2, 3 };

            Assert.Throws<ArgumentNullException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.InsertRangeSorted(null, comparison, notificationMode);
                }
            });
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedNullComparisonTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithValidRangeAndNullComparison_InForeach_ThrowsCorrectException(
            IEnumerable<int> insertItems,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>() { 1, 2, 3 };

            Assert.Throws<ArgumentNullException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.InsertRangeSorted(insertItems, null, notificationMode);
                }
            });
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedWrongModeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithValidRangeAndValidComparisonAndInvalidNotificationMode_InForeach_ThrowsCorrectException(
            IEnumerable<int> insertItems,
            Comparison<int> comparison,
            NotifyCollectionChangedAction notificationMode)
        {
            var collection = new ObservableRangeCollection<int>() { 1, 2, 3 };

            Assert.Throws<ArgumentException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.InsertRangeSorted(insertItems, comparison, notificationMode);
                }
            });
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedNonEmptyCollectionWithEmptyRangeWithValidComparisonWithValidModeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithEmptyRangeAndValidComparisonAndValidNotificationMode_InForeach_DoesNotThrowException(
            ObservableRangeCollection<int> collection,
            Comparison<int> comparison,
            NotifyCollectionChangedAction notificationMode)
        {
            var items = collection.ToList();
            foreach (var item in collection)
            {
                collection.InsertRangeSorted(new List<int>(), comparison, notificationMode);
            }

            Assert.Equal(items, collection);
        }

        [Theory]
        [MemberData(
            nameof(ObservableRangeCollectionDataProvider.InsertRangeSortedNonEmptyCollectionWithNonEmptyRangeWithValidComparisonWithValidModeTestData),
            MemberType = typeof(ObservableRangeCollectionDataProvider))]
        public void InsertRangeSorted_WhenCalledWithNonEmptyRangeAndValidComparisonAndValidNotificationMode_InForeach_ThrowsCorrectException(
            ObservableRangeCollection<int> collection,
            IEnumerable<int> insertItems,
            Comparison<int> comparison,
            NotifyCollectionChangedAction notificationMode)
        {
            Assert.Throws<InvalidOperationException>(() =>
            {
                foreach (var item in collection)
                {
                    collection.InsertRangeSorted(insertItems, comparison, notificationMode);
                }
            });
        }

        #endregion
    }
}
