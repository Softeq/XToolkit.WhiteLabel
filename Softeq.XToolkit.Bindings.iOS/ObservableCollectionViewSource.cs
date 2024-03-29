﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using Foundation;
using Softeq.XToolkit.Bindings.iOS.Extensions;
using Softeq.XToolkit.Common;
using UIKit;

#nullable disable

namespace Softeq.XToolkit.Bindings.iOS
{
    /// <summary>
    ///     A <see cref="T:UIKit.UICollectionViewSource" /> that automatically updates
    ///     the associated <see cref="T:UIKit.UICollectionView" /> when its
    ///     data source changes. Note that the changes are only observed if the data source
    ///     implements <see cref="T:System.Collections.Specialized.INotifyCollectionChanged" />.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the data source.</typeparam>
    /// <typeparam name="TCell">
    ///     The type of the <see cref="T:UIKit.UICollectionViewCell" /> used in the CollectionView.
    ///     This can either be UICollectionViewCell or a derived type.
    /// </typeparam>
    public class ObservableCollectionViewSource<TItem, TCell> : UICollectionViewSource, INotifyPropertyChanged
        where TCell : UICollectionViewCell
    {
        /// <summary>
        ///     The real count of items in a <see cref="T:UIKit.UICollectionView" /> with infinite scroll.
        /// </summary>
        public const int InfiniteItemsCount = 100000;

        protected const string DefaultReuseId = "C";

        private IList<TItem> _dataSource;
        private INotifyCollectionChanged _notifier;
        private NSString _reuseId;
        private TItem _selectedItem;
        private UICollectionView _view;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableCollectionViewSource{TItem, TCell}"/> class.
        /// </summary>
        public ObservableCollectionViewSource()
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableCollectionViewSource{TItem, TCell}"/> class
        ///     with a value for <see cref="IsInfiniteScroll"/> flag.
        /// </summary>
        /// <param name="canBeScrolledInfinitely">Enable infinite scroll.</param>
        public ObservableCollectionViewSource(bool canBeScrolledInfinitely)
            : this()
        {
            IsInfiniteScroll = canBeScrolledInfinitely;
        }

        /// <summary>
        ///     Occurs when a property of this instance changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Occurs when a item was clicked in the UICollectionView.
        /// </summary>
        public event EventHandler<GenericEventArgs<TItem>> ItemClicked;

        /// <summary>
        ///     Occurs when a new item gets selected in the UICollectionView.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        ///     Gets or sets a delegate to a method taking a <see cref="T:UIKit.UICollectionViewCell" />
        ///     and setting its elements' properties according to the item
        ///     passed as second parameter.
        /// </summary>
        public Action<TCell, TItem, NSIndexPath> BindCellDelegate { get; set; }

        /// <summary>
        ///     Gets a value indicating whether this <see cref="T:UIKit.UICollectionViewCell" />
        ///     can be scrolled infinitely.
        /// </summary>
        public bool IsInfiniteScroll { get; }

        /// <summary>
        ///     Gets or sets the data source of this list controller.
        /// </summary>
        public IList<TItem> DataSource
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
                    _notifier.CollectionChanged -= HandleCollectionChanged;
                }

                _dataSource = value;
                _notifier = value as INotifyCollectionChanged;

                if (_notifier != null)
                {
                    _notifier.CollectionChanged += HandleCollectionChanged;
                }

                if (_view != null)
                {
                    _view.ReloadData();
                }
            }
        }

        /// <summary>
        ///     Gets or sets a delegate to a method returning a <see cref="T:UIKit.UICollectionReusableView" />
        ///     and used to set supplementary views on the UICollectionView.
        /// </summary>
        public Func<NSString, NSIndexPath, UICollectionReusableView> GetSupplementaryViewDelegate { get; set; }

        /// <summary>
        ///     Gets or sets a reuse identifier for the UICollectionView's cells.
        /// </summary>
        public string ReuseId
        {
            get => NsReuseId.ToString();

            set => _reuseId = string.IsNullOrEmpty(value) ? null : new NSString(value);
        }

        /// <summary>
        ///     Gets or sets the UICollectionView's selected item. You can use one-way databinding on this property.
        /// </summary>
        public TItem SelectedItem
        {
            get => _selectedItem;

            protected set
            {
                if (Equals(_selectedItem, value))
                {
                    return;
                }

                _selectedItem = value;
                RaisePropertyChanged(nameof(SelectedItem));
                SelectionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        protected NSString NsReuseId => _reuseId ?? new NSString(DefaultReuseId);

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UICollectionViewSource.GetCell" /> method.
        ///     Creates and returns a cell for the UICollectionView. Where needed, this method will
        ///     optimize the reuse of cells for a better performance.
        /// </summary>
        /// <param name="collectionView">The UICollectionView associated to this source.</param>
        /// <param name="indexPath">The NSIndexPath pointing to the item for which the cell must be returned.</param>
        /// <returns>The created and initialised <see cref="T:UIKit.UICollectionViewCell" />.</returns>
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (TCell) collectionView.DequeueReusableCell(NsReuseId, indexPath);

            try
            {
                if (_dataSource != null)
                {
                    var item = GetItem(indexPath);
                    BindCell(cell, item, indexPath);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return cell;
        }

        /// <summary>
        ///     Gets the item selected by the NSIndexPath passed as parameter.
        /// </summary>
        /// <param name="indexPath">The NSIndexPath pointing to the desired item.</param>
        /// <returns>The item selected by the NSIndexPath passed as parameter.</returns>
        public TItem GetItem(NSIndexPath indexPath)
        {
            var index = IsInfiniteScroll ? indexPath.Row % _dataSource.Count : indexPath.Row;
            return _dataSource[index];
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UICollectionViewSource.GetItemsCount" /> method.
        ///     Gets the number of items in the data source.
        /// </summary>
        /// <param name="collectionView">The UICollectionView associated to this source.</param>
        /// <param name="section">
        ///     The section for which the count is needed. In the current
        ///     implementation, only one section is supported.
        /// </param>
        /// <returns>The number of items in the data source.</returns>
        public override nint GetItemsCount(UICollectionView collectionView, nint section)
        {
            SetView(collectionView);
            if (_dataSource.Count == 0)
            {
                return 0;
            }

            return IsInfiniteScroll ? InfiniteItemsCount : _dataSource.Count;
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UICollectionViewSource.GetViewForSupplementaryElement" /> method.
        ///     When called, checks if the <see cref="GetSupplementaryViewDelegate" />
        ///     delegate has been set. If yes, calls that delegate to get a supplementary view for the UICollectionView.
        /// </summary>
        /// <param name="collectionView">The UICollectionView associated to this source.</param>
        /// <param name="elementKind">The kind of supplementary element.</param>
        /// <param name="indexPath">The NSIndexPath pointing to the element.</param>
        /// <returns>A supplementary view for the UICollectionView.</returns>
        public override UICollectionReusableView GetViewForSupplementaryElement(
            UICollectionView collectionView,
            NSString elementKind,
            NSIndexPath indexPath)
        {
            if (GetSupplementaryViewDelegate == null)
            {
                throw new InvalidOperationException(
                    "GetViewForSupplementaryElement was called but no GetSupplementaryViewDelegate was found");
            }

            var view = GetSupplementaryViewDelegate(elementKind, indexPath);
            return view;
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UICollectionViewSource.ItemDeselected" /> method.
        ///     Called when an item is deselected in the UICollectionView.
        ///     <remark>
        ///         If you subclass ObservableCollectionViewSource, you may override this method
        ///         but you may NOT call base.ItemDeselected(...) in your overriden method, as this causes an exception
        ///         in iOS. Because of this, you must take care of resetting the <see cref="SelectedItem" /> property
        ///         yourself by calling SelectedItem = default(TItem);
        ///     </remark>
        /// </summary>
        /// <param name="collectionView">The UICollectionView associated to this source.</param>
        /// <param name="indexPath">The NSIndexPath pointing to the element.</param>
        public override void ItemDeselected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            SelectedItem = default;
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UICollectionViewSource.ItemSelected" /> method.
        ///     Called when an item is selected in the UICollectionView.
        ///     <remark>
        ///         If you subclass ObservableCollectionViewSource, you may override this method
        ///         but you may NOT call base.ItemSelected(...) in your overriden method, as this causes an exception
        ///         in iOS. Because of this, you must take care of setting the <see cref="SelectedItem" /> property
        ///         yourself by calling var item = GetItem(indexPath); SelectedItem = item;
        ///     </remark>
        /// </summary>
        /// <param name="collectionView">The UICollectionView associated to this source.</param>
        /// <param name="indexPath">The NSIndexPath pointing to the element.</param>
        public override void ItemSelected(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var item = GetItem(indexPath);
            SelectedItem = item;
            ItemClicked?.Invoke(this, new GenericEventArgs<TItem>(item));
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UICollectionViewSource.NumberOfSections" /> method.
        ///     The number of sections in this UICollectionView. In the current implementation,
        ///     only one section is supported.
        /// </summary>
        /// <param name="collectionView">The UICollectionView associated to this source.</param>
        /// <returns>Returns the number of sections in the <paramref name="collectionView"/>.</returns>
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            SetView(collectionView);
            return 1;
        }

        /// <summary>
        ///     Sets a <see cref="T:UIKit.UICollectionViewCell" />'s elements according to an item's properties.
        ///     If a <see cref="BindCellDelegate" /> is available, this delegate will be used.
        ///     If not, a simple text will be shown.
        /// </summary>
        /// <param name="cell">The cell that will be prepared.</param>
        /// <param name="item">The item that should be used to set the cell up.</param>
        /// <param name="indexPath">The <see cref="T:Foundation.NSIndexPath" /> for this cell.</param>
        protected virtual void BindCell(UICollectionViewCell cell, object item, NSIndexPath indexPath)
        {
            if (BindCellDelegate == null)
            {
                throw new InvalidOperationException(
                    "BindCell was called but no BindCellDelegate was found");
            }

            BindCellDelegate((TCell) cell, (TItem) item, indexPath);
        }

        /// <summary>
        ///     Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void RaisePropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;

            if (handler != null)
            {
                handler.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (_view == null)
            {
                return;
            }

            NSThreadExtensions.ExecuteOnMainThread(() =>
            {
                if (IsInfiniteScroll)
                {
                    _view.ReloadData();
                    return;
                }

                switch (e.Action)
                {
                    case NotifyCollectionChangedAction.Add:
                        {
                            var count = e.NewItems.Count;
                            var paths = new NSIndexPath[count];

                            for (var i = 0; i < count; i++)
                            {
                                paths[i] = NSIndexPath.FromItemSection(e.NewStartingIndex + i, 0);
                            }

                            _view.InsertItems(paths);
                        }

                        break;

                    case NotifyCollectionChangedAction.Remove:
                        {
                            var count = e.OldItems.Count;
                            var paths = new NSIndexPath[count];

                            for (var i = 0; i < count; i++)
                            {
                                var index = NSIndexPath.FromItemSection(e.OldStartingIndex + i, 0);
                                paths[i] = index;

                                var item = e.OldItems[i];

                                if (Equals(SelectedItem, item))
                                {
                                    SelectedItem = default;
                                }
                            }

                            _view.DeleteItems(paths);
                        }

                        break;

                    case NotifyCollectionChangedAction.Move:
                        {
                            if (e.NewItems.Count != 1 || e.OldItems.Count != 1)
                            {
                                _view.ReloadData();
                            }
                            else if (e.NewStartingIndex != e.OldStartingIndex)
                            {
                                _view.MoveItem(
                                    NSIndexPath.FromItemSection(e.OldStartingIndex, 0),
                                    NSIndexPath.FromItemSection(e.NewStartingIndex, 0));
                            }
                        }

                        break;

                    default:
                        _view.ReloadData();
                        break;
                }
            });
        }

        private void SetView(UICollectionView collectionView)
        {
            if (_view != null)
            {
                return;
            }

            _view = collectionView;
        }
    }
}
