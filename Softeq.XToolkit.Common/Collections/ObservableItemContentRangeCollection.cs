// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace Softeq.XToolkit.Common.Collections
{
    /// <summary>
    ///     Dynamic data collection that provides notifications
    ///     when items get added, removed, changed or when the whole list is refreshed.
    ///     <para/>
    ///     The key difference from the <see cref="ObservableRangeCollection{T}"/>
    ///     is that this collection notifies listeners when the collection items are changed.
    /// </summary>
    /// <typeparam name="T">Collection elements type.</typeparam>
    public class ObservableItemContentRangeCollection<T> : ObservableRangeCollection<T> where T : INotifyPropertyChanged
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableItemContentRangeCollection{T}"/> class.
        /// </summary>
        public ObservableItemContentRangeCollection()
        {
            CollectionChanged += ObservableItemContentRangeCollectionChanged;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableItemContentRangeCollection{T}"/> class
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
        public ObservableItemContentRangeCollection(IEnumerable<T> collection)
            : this()
        {
            AddRange(collection);
        }

        private void ObservableItemContentRangeCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    ((INotifyPropertyChanged) item).PropertyChanged += ItemPropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    ((INotifyPropertyChanged) item).PropertyChanged -= ItemPropertyChanged;
                }
            }
        }

        private void ItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var args = new NotifyCollectionChangedEventArgs(
                NotifyCollectionChangedAction.Replace,
                sender,
                sender,
                IndexOf((T) sender));
            OnCollectionChanged(args);
        }
    }
}
