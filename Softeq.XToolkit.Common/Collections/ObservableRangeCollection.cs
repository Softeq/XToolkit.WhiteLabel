// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Softeq.XToolkit.Common.Collections
{
    /// <summary>
    ///     Represents a dynamic data collection that provides notifications
    ///     when items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">The element type of the collection.</typeparam>
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableRangeCollection{T}"/> class.
        /// </summary>
        public ObservableRangeCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableRangeCollection{T}"/> class that contains
        ///     elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <exception cref="T:System.ArgumentNullException">The collection parameter cannot be null.</exception>
        public ObservableRangeCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        ///     Adds the elements of the specified collection to the end of the ObservableCollection(Of T).
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <param name="notificationMode">Action that will called after adding.</param>
        public void AddRange(
            IEnumerable<T> collection,
            NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
        {
            if (notificationMode != NotifyCollectionChangedAction.Add &&
                notificationMode != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException("Mode must be either Add or Reset for AddRange.", nameof(notificationMode));
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                foreach (var i in collection)
                {
                    Items.Add(i);
                }

                OnPropertyChanged(EventArgsCache.CountPropertyChanged);
                OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
                OnCollectionChanged(EventArgsCache.ResetCollectionChanged);

                return;
            }

            var startIndex = Count;
            var list = collection as List<T>;
            var changedItems = list ?? new List<T>(collection);
            foreach (var i in changedItems)
            {
                Items.Add(i);
            }

            OnPropertyChanged(EventArgsCache.CountPropertyChanged);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));
        }

        /// <summary>
        ///     Insert the elements of the specified collection at a given index of the ObservableCollection(Of T).
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <param name="startIndex">The index in the array at which to insert items.</param>
        public void InsertRange(IEnumerable<T> collection, int startIndex)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (startIndex < 0 || startIndex > Count)
            {
                throw new IndexOutOfRangeException(nameof(startIndex));
            }

            CheckReentrancy();

            var indexToInsert = startIndex;
            var list = collection as List<T>;
            var changedItems = list ?? new List<T>(collection);
            foreach (var i in changedItems)
            {
                Items.Insert(indexToInsert++, i);
            }

            OnPropertyChanged(EventArgsCache.CountPropertyChanged);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, changedItems, startIndex));
        }

        /// <summary>
        ///     Insert the elements of the specified collection and sort the collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        /// <param name="comparer">Method that compares <typeparamref name="T" /> objects.</param>
        /// <param name="notificationMode">Action that will called after deletion.</param>
        /// <returns>Inserted items indexes.</returns>
        public IList<int> InsertRangeSorted(
            IEnumerable<T> collection,
            Comparison<T> comparer,
            NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
        {
            if (notificationMode != NotifyCollectionChangedAction.Add &&
                notificationMode != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException("Mode must be either Add or Reset for InsertRangeSorted.", nameof(notificationMode));
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            CheckReentrancy();

            var itemsList = new List<T>(collection);
            itemsList.Sort(comparer);
            var insertedItemsIndexes = new List<int>();
            foreach (var item in itemsList)
            {
                var i = 0;
                while (i < Items.Count && comparer(Items[i], item) <= 0)
                {
                    i++;
                }

                insertedItemsIndexes.Add(i);
                Items.Insert(i, item);
            }

            OnPropertyChanged(EventArgsCache.CountPropertyChanged);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);

            if (notificationMode == NotifyCollectionChangedAction.Add)
            {
                OnCollectionChanged(new NotifyCollectionInsertEventArgs(insertedItemsIndexes));
            }
            else
            {
                OnCollectionChanged(new NotifyCollectionChangedEventArgs(notificationMode));
            }

            return insertedItemsIndexes;
        }

        /// <summary>
        ///     Removes the first occurence of each item in the specified collection from ObservableCollection(Of T). NOTE: with
        ///     notificationMode = Remove, removed items starting index is not set because items are not guaranteed to be
        ///     consecutive.
        /// </summary>
        /// <param name="collection">The collection of items to delete.</param>
        /// <param name="notificationMode">Action that will called after deletion.</param>
        public void RemoveRange(
            IEnumerable<T> collection,
            NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Reset)
        {
            if (notificationMode != NotifyCollectionChangedAction.Remove &&
                notificationMode != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException(
                    "Mode must be either Remove or Reset for RemoveRange.",
                    nameof(notificationMode));
            }

            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                foreach (var i in collection)
                {
                    Items.Remove(i);
                }

                OnCollectionChanged(EventArgsCache.ResetCollectionChanged);

                return;
            }

            var changedItems = collection is List<T> ? (List<T>) collection : new List<T>(collection);
            var index = Items.IndexOf(changedItems[0]);

            for (var i = 0; i < changedItems.Count; i++)
            {
                if (!Items.Remove(changedItems[i]))
                {
                    changedItems.RemoveAt(i); // Can't use a foreach because changedItems is intended to be (carefully) modified
                    i--;
                }
            }

            OnPropertyChanged(EventArgsCache.CountPropertyChanged);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, changedItems, index));
        }

        /// <summary>
        ///     Clears the current collection and replaces it with the specified item.
        /// </summary>
        /// <param name="item">Item that will added to collection.</param>
        public void Replace(T item)
        {
            ReplaceRange(new[] { item });
        }

        /// <summary>
        ///     Clears the current collection and replaces it with the specified collection.
        /// </summary>
        /// <param name="collection">The collection from which the elements are copied.</param>
        public void ReplaceRange(IEnumerable<T> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            Items.Clear();
            AddRange(collection, NotifyCollectionChangedAction.Reset);
        }
    }

    internal static class EventArgsCache
    {
        internal static readonly PropertyChangedEventArgs CountPropertyChanged = new PropertyChangedEventArgs("Count");
        internal static readonly PropertyChangedEventArgs IndexerPropertyChanged = new PropertyChangedEventArgs("Item[]");

        internal static readonly NotifyCollectionChangedEventArgs ResetCollectionChanged =
            new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
    }
}
