﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;
using Foundation;
using Softeq.XToolkit.Common.EventArguments;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    /// <summary>
    ///     A <see cref="UICollectionViewSource" /> that automatically updates the associated <see cref="UICollectionView" />
    ///     when its
    ///     data source changes. Note that the changes are only observed if the data source
    ///     implements <see cref="INotifyCollectionChanged" />.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the data source.</typeparam>
    /// <typeparam name="TCell">
    ///     The type of the <see cref="UICollectionViewCell" /> used in the CollectionView.
    ///     This can either be UICollectionViewCell or a derived type.
    /// </typeparam>
    public class ObservableCollectionViewSource<TItem, TCell> : UICollectionViewSource, INotifyPropertyChanged
        where TCell : UICollectionViewCell
    {
        /// <summary>
        ///     The real count of items in a <see cref="UICollectionView" /> with infinite scroll.
        /// </summary>
        public const int InfiniteItemsCount = 100000;

        private readonly NSString _defaultReuseId = new NSString("C");
        private readonly Thread _mainThread;

        private IList<TItem> _dataSource;
        private INotifyCollectionChanged _notifier;
        private NSString _reuseId;
        private TItem _selectedItem;
        private UICollectionView _view;

        public event EventHandler<GenericEventArgs<TItem>> ItemClicked;

        /// <summary>
        ///     Creates and initializes a new instance of <see cref="ObservableCollectionViewSource{TItem, TCell}" />
        /// </summary>
        public ObservableCollectionViewSource()
        {
            _mainThread = Thread.CurrentThread;
        }

        /// <summary>
        ///     Creates and initializes a new instance of <see cref="ObservableCollectionViewSource{TItem, TCell}" />
        ///     with a value for IsInfiniteScroll flag.
        /// </summary>
        public ObservableCollectionViewSource(bool canBeScrolledInfinitely) : this()
        {
            IsInfiniteScroll = canBeScrolledInfinitely;
        }

        /// <summary>
        ///     A delegate to a method taking a <see cref="UICollectionViewCell" />
        ///     and setting its elements' properties according to the item
        ///     passed as second parameter.
        /// </summary>
        public Action<TCell, TItem, NSIndexPath> BindCellDelegate { get; set; }

        /// <summary>
        ///     Indicates whether this <see cref="UICollectionViewCell" />
        ///     can be scrolled infinitely.
        /// </summary>
        public bool IsInfiniteScroll { get; }

        /// <summary>
        ///     The data source of this list controller.
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
        ///     A delegate to a method returning a <see cref="UICollectionReusableView" />
        ///     and used to set supplementary views on the UICollectionView.
        /// </summary>
        public Func<NSString, NSIndexPath, UICollectionReusableView> GetSupplementaryViewDelegate { get; set; }

        /// <summary>
        ///     A reuse identifier for the UICollectionView's cells.
        /// </summary>
        public string ReuseId
        {
            get => NsReuseId.ToString();

            set => _reuseId = string.IsNullOrEmpty(value) ? null : new NSString(value);
        }

        /// <summary>
        ///     Gets the UICollectionView's selected item. You can use one-way databinding on this property.
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

        private NSString NsReuseId => _reuseId ?? _defaultReuseId;

        /// <summary>
        ///     Occurs when a property of this instance changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Overrides the <see cref="UICollectionViewSource.GetCell" /> method.
        ///     Creates and returns a cell for the UICollectionView. Where needed, this method will
        ///     optimize the reuse of cells for a better performance.
        /// </summary>
        /// <param name="collectionView">The UICollectionView associated to this source.</param>
        /// <param name="indexPath">The NSIndexPath pointing to the item for which the cell must be returned.</param>
        /// <returns>The created and initialised <see cref="UICollectionViewCell" />.</returns>
        public override UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (TCell)collectionView.DequeueReusableCell(NsReuseId, indexPath);

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
        ///     Overrides the <see cref="UICollectionViewSource.GetItemsCount" /> method.
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
        ///     Overrides the <see cref="UICollectionViewSource.GetViewForSupplementaryElement" /> method.
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
        ///     Overrides the <see cref="UICollectionViewSource.ItemDeselected" /> method.
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
            SelectedItem = default(TItem);
        }

        /// <summary>
        ///     Overrides the <see cref="UICollectionViewSource.ItemSelected" /> method.
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
        ///     Overrides the <see cref="UICollectionViewSource.NumberOfSections" /> method.
        ///     The number of sections in this UICollectionView. In the current implementation,
        ///     only one section is supported.
        /// </summary>
        /// <param name="collectionView">The UICollectionView associated to this source.</param>
        /// <returns></returns>
        public override nint NumberOfSections(UICollectionView collectionView)
        {
            SetView(collectionView);
            return 1;
        }

        /// <summary>
        ///     Sets a <see cref="UICollectionViewCell" />'s elements according to an item's properties.
        ///     If a <see cref="BindCellDelegate" /> is available, this delegate will be used.
        ///     If not, a simple text will be shown.
        /// </summary>
        /// <param name="cell">The cell that will be prepared.</param>
        /// <param name="item">The item that should be used to set the cell up.</param>
        /// <param name="indexPath">The <see cref="NSIndexPath" /> for this cell.</param>
        protected virtual void BindCell(UICollectionViewCell cell, object item, NSIndexPath indexPath)
        {
            if (BindCellDelegate == null)
            {
                throw new InvalidOperationException(
                    "BindCell was called but no BindCellDelegate was found");
            }

            BindCellDelegate((TCell)cell, (TItem)item, indexPath);
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

            Action act = () =>
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
                                paths[i] = NSIndexPath.FromRowSection(e.NewStartingIndex + i, 0);
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
                                var index = NSIndexPath.FromRowSection(e.OldStartingIndex + i, 0);
                                paths[i] = index;

                                var item = e.OldItems[i];

                                if (Equals(SelectedItem, item))
                                {
                                    SelectedItem = default(TItem);
                                }
                            }

                            _view.DeleteItems(paths);
                        }
                        break;

                    default:
                        _view.ReloadData();
                        break;
                }
            };

            var isMainThread = Thread.CurrentThread == _mainThread;

            if (isMainThread)
            {
                act();
            }
            else
            {
                NSOperationQueue.MainQueue.AddOperation(act);
                NSOperationQueue.MainQueue.WaitUntilAllOperationsAreFinished();
            }
        }

        private void SetView(UICollectionView collectionView)
        {
            if (_view != null)
            {
                return;
            }

            _view = collectionView;
        }

        /// <summary>
        ///     Occurs when a new item gets selected in the UICollectionView.
        /// </summary>
        public event EventHandler SelectionChanged;
    }
}