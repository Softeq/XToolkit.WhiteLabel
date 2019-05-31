// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Android.Views;
using Android.Widget;

namespace Softeq.XToolkit.Bindings.Droid
{
    /// <summary>
    ///     A <see cref="BaseAdapter{T}" /> that can be used with an Android ListView. After setting
    ///     the <see cref="DataSource" /> and the <see cref="GetTemplateDelegate" /> properties, the adapter is
    ///     suitable for a list control. If the DataSource is an <see cref="INotifyCollectionChanged" />,
    ///     changes to the collection will be observed and the UI will automatically be updated.
    /// </summary>
    /// <typeparam name="T">The type of the items contained in the <see cref="DataSource" />.</typeparam>
    public class ObservableAdapter<T> : BaseAdapter<T>
    {
        private IList<T> _dataSource;
        private INotifyCollectionChanged _notifier;

        /// <summary>
        ///     Gets the number of items in the DataSource.
        /// </summary>
        public override int Count => _dataSource?.Count ?? 0;

        /// <summary>
        ///     Gets or sets the list containing the items to be represented in the list control.
        /// </summary>
        public IList<T> DataSource
        {
            get => _dataSource;
            set
            {
                if (Equals(_dataSource, value))
                {
                    return;
                }

                if (_notifier != null)
                {
                    _notifier.CollectionChanged -= NotifierCollectionChanged;
                }

                _dataSource = value;
                _notifier = _dataSource as INotifyCollectionChanged;

                if (_notifier != null)
                {
                    _notifier.CollectionChanged += NotifierCollectionChanged;
                }
            }
        }

        /// <summary>
        ///     Gets and sets a method taking an item's position in the list, the item itself,
        ///     and a recycled Android View, and returning an adapted View for this item. Note that the recycled
        ///     view might be null, in which case a new View must be inflated by this method.
        /// </summary>
        public Func<int, T, View, View> GetTemplateDelegate { get; set; }

        /// <summary>
        ///     Gets the item corresponding to the index in the DataSource.
        /// </summary>
        /// <param name="index">The index of the item that needs to be returned.</param>
        /// <returns>The item corresponding to the index in the DataSource</returns>
        public override T this[int index] => _dataSource == null ? default(T) : _dataSource[index];

        /// <summary>
        ///     Returns a unique ID for the item corresponding to the position parameter.
        ///     In this implementation, the method always returns the position itself.
        /// </summary>
        /// <param name="position">The position of the item for which the ID needs to be returned.</param>
        /// <returns>A unique ID for the item corresponding to the position parameter.</returns>
        public override long GetItemId(int position)
        {
            return position;
        }

        /// <summary>
        ///     Prepares the view (template) for the item corresponding to the position
        ///     in the DataSource. This method calls the <see cref="GetTemplateDelegate" /> method so that the caller
        ///     can create (if necessary) and adapt the template for the corresponding item.
        /// </summary>
        /// <param name="position">The position of the item in the DataSource.</param>
        /// <param name="convertView">
        ///     A recycled view. If this parameter is null,
        ///     a new view must be inflated.
        /// </param>
        /// <param name="parent">The view's parent.</param>
        /// <returns>A view adapted for the item at the corresponding position.</returns>
        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            if (GetTemplateDelegate == null)
            {
                return convertView;
            }

            var item = _dataSource[position];
            var view = GetTemplateDelegate(position, item, convertView);
            return view;
        }

        private void NotifierCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            NotifyDataSetChanged();
        }
    }
}