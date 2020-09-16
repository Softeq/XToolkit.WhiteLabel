// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Softeq.XToolkit.Common.Collections.EventArgs;

namespace Softeq.XToolkit.Common.Collections
{
    /// <summary>
    ///     Dynamic data collection that provides notifications
    ///     when items get added, removed, or when the whole list is refreshed.
    /// </summary>
    /// <typeparam name="T">Collection elements type.</typeparam>
    public class ObservableRangeCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableRangeCollection{T}"/> class.
        /// </summary>
        public ObservableRangeCollection()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableRangeCollection{T}"/> class
        ///     that contains elements copied from the specified collection.
        /// </summary>
        /// <param name="collection">
        ///     The collection from which the elements are copied.
        ///     <para/>
        ///     The collection itself cannot be <see langword="null"/>,
        ///     but it can contain elements that are <see langword="null"/>,
        ///     if type T is a reference type.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        public ObservableRangeCollection(IEnumerable<T> collection)
            : base(collection)
        {
        }

        /// <summary>
        ///     Adds the elements of the specified collection
        ///     to the end of the current <see cref="ObservableRangeCollection{T}"/>.
        /// </summary>
        /// <param name="collection">
        ///     The collection from which the elements are copied.
        ///     <para/>
        ///     The collection itself cannot be <see langword="null"/>,
        ///     but it can contain elements that are <see langword="null"/>,
        ///     if type T is a reference type.
        /// </param>
        /// <param name="notificationMode">
        ///     Description of an action that will be added to
        ///     <see cref="ObservableCollection{T}.CollectionChanged"/> event.
        ///     <para/>
        ///     Can be only <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Add"/>
        ///     or <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/>.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="notificationMode"/> parameter must be either
        ///     <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Add"/>
        ///     or <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/>.
        /// </exception>
        public void AddRange(
            IEnumerable<T> collection,
            NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (notificationMode != NotifyCollectionChangedAction.Add &&
                notificationMode != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException("Notification mode must be either Add or Reset for AddRange.", nameof(notificationMode));
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
        ///     Insert the elements of the specified collection
        ///     at a given index of the current <see cref="ObservableRangeCollection{T}"/>.
        /// </summary>
        /// <param name="collection">
        ///     The collection from which the elements are copied.
        ///     <para/>
        ///     The collection itself cannot be <see langword="null"/>,
        ///     but it can contain elements that are <see langword="null"/>,
        ///     if type T is a reference type.
        /// </param>
        /// <param name="startIndex">
        ///     The zero-based index at which the new elements should be inserted.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///     <paramref name="startIndex"/> parameter must be less than Count and greater than or equal to 0.
        /// </exception>
        public void InsertRange(IEnumerable<T> collection, int startIndex)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (startIndex < 0 || startIndex > Count)
            {
                throw new ArgumentOutOfRangeException(nameof(startIndex));
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
        ///     Insert the elements of the specified collection in the ascending sort order.
        ///     <para/>
        ///     This method assumes that the current <see cref="ObservableRangeCollection{T}"/> is already sorted in ascending order.
        /// </summary>
        /// <param name="collection">
        ///     The collection from which the elements are copied.
        ///     <para/>
        ///     The collection itself cannot be <see langword="null"/>,
        ///     but it can contain elements that are <see langword="null"/>,
        ///     if type T is a reference type.
        /// </param>
        /// <param name="comparison">
        ///     Method that compares <typeparamref name="T" /> objects.
        ///     <para/>
        ///     Cannot be <see langword="null"/>.
        /// </param>
        /// <param name="notificationMode">
        ///     Description of an action that will be added to
        ///     <see cref="ObservableCollection{T}.CollectionChanged"/> event.
        ///     <para/>
        ///     Can be only <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Add"/>
        ///     or <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/>.
        /// </param>
        /// <returns>Inserted items indexes.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> and <paramref name="comparison"/> parameters cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="notificationMode"/> parameter must be either
        ///     <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Add"/>
        ///     or <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/>.
        /// </exception>
        public IList<int> InsertRangeSorted(
            IEnumerable<T> collection,
            Comparison<T> comparison,
            NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Add)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (comparison == null)
            {
                throw new ArgumentNullException(nameof(comparison));
            }

            if (notificationMode != NotifyCollectionChangedAction.Add &&
                notificationMode != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException("Notification mode must be either Add or Reset for InsertRangeSorted.", nameof(notificationMode));
            }

            CheckReentrancy();

            var itemsList = new List<T>(collection);
            itemsList.Sort(comparison);
            var insertedItemsIndexes = new List<int>();
            foreach (var item in itemsList)
            {
                var i = 0;
                while (i < Items.Count && comparison(Items[i], item) <= 0)
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
        ///     Removes the first occurence of each item in the specified collection
        ///     from the current <see cref="ObservableRangeCollection{T}"/>.
        ///     Removed items are not guaranteed to be consecutive.
        ///     <para/>
        ///     With <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/> removed items starting index is not set.
        ///     <para/>
        ///     With <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Remove"/> removed items starting index is set to the index of the first removed element.
        /// </summary>
        /// <param name="collection">
        ///     The collection of items to delete.
        ///     <para/>
        ///     The collection itself cannot be <see langword="null"/>,
        ///     but it can contain elements that are <see langword="null"/>,
        ///     if type T is a reference type.
        /// </param>
        /// <param name="notificationMode">
        ///     Description of an action that will be added to
        ///     <see cref="ObservableCollection{T}.CollectionChanged"/> event.
        ///     <para/>
        ///     Can be only <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Remove"/>
        ///     or <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/>.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> parameter cannot be <see langword="null"/>.
        /// </exception>
        /// <exception cref="T:System.ArgumentException">
        ///     <paramref name="notificationMode"/> parameter must be either
        ///     <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Remove"/>
        ///     or <see cref="F:System.Collections.Specialized.NotifyCollectionChangedAction.Reset"/>.
        /// </exception>
        public void RemoveRange(
            IEnumerable<T> collection,
            NotifyCollectionChangedAction notificationMode = NotifyCollectionChangedAction.Reset)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            if (notificationMode != NotifyCollectionChangedAction.Remove &&
                notificationMode != NotifyCollectionChangedAction.Reset)
            {
                throw new ArgumentException("Notification mode must be either Remove or Reset for RemoveRange.", nameof(notificationMode));
            }

            CheckReentrancy();

            if (notificationMode == NotifyCollectionChangedAction.Reset)
            {
                foreach (var item in collection)
                {
                    Items.Remove(item);
                }

                OnCollectionChanged(EventArgsCache.ResetCollectionChanged);

                return;
            }

            var removedItems = new List<T>();
            var firstRemovedItemIndex = -1;
            foreach (var item in collection)
            {
                if (firstRemovedItemIndex == -1)
                {
                    firstRemovedItemIndex = Items.IndexOf(item);
                }

                if (Items.Remove(item))
                {
                    removedItems.Add(item);
                }
            }

            OnPropertyChanged(EventArgsCache.CountPropertyChanged);
            OnPropertyChanged(EventArgsCache.IndexerPropertyChanged);
            OnCollectionChanged(
                new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, removedItems, firstRemovedItemIndex));
        }

        /// <summary>
        ///     Clears the current <see cref="ObservableRangeCollection{T}"/>
        ///     and replaces it with the specified item.
        /// </summary>
        /// <param name="item">Item that will added to the collection.</param>
        public void Replace(T item)
        {
            ReplaceRange(new[] { item });
        }

        /// <summary>
        ///     Clears the current <see cref="ObservableRangeCollection{T}"/>
        ///     and replaces it with the items from the specified collection.
        /// </summary>
        /// <param name="collection">
        ///     The collection from which the elements are copied.
        ///     <para/>
        ///     The collection itself cannot be <see langword="null"/>,
        ///     but it can contain elements that are <see langword="null"/>,
        ///     if type T is a reference type.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="collection"/> parameter cannot be <see langword="null"/>.
        /// </exception>
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
