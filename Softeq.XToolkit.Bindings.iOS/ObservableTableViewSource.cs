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
    ///     A <see cref="T:UIKit.UITableViewSource" /> that automatically updates the associated
    ///     <see cref="T:UIKit.UITableView" /> when its data source changes. Note that the changes are only observed
    ///     if the data source implements <see cref="T:System.Collections.Specialized.INotifyCollectionChanged" />.
    /// </summary>
    /// <typeparam name="TItem">The type of the items in the data source.</typeparam>
    public class ObservableTableViewSource<TItem> : UITableViewSource, INotifyPropertyChanged
    {
        /// <summary>
        ///     The <see cref="SelectedItem" /> property's name.
        /// </summary>
        public const string SelectedItemPropertyName = "SelectedItem";

        protected const string DefaultReuseId = "C";

        private IList<TItem> _dataSource;
        private INotifyCollectionChanged _notifier;
        private NSString _reuseId;
        private TItem _selectedItem;
        private UITableView _view;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ObservableTableViewSource{TItem}"/> class.
        /// </summary>
        public ObservableTableViewSource()
        {
            AddAnimation = UITableViewRowAnimation.Automatic;
            DeleteAnimation = UITableViewRowAnimation.Automatic;
        }

        public ObservableTableViewSource(IList<TItem> items)
            : this()
        {
            DataSource = items;
        }

        /// <summary>
        ///     Gets or sets which animation should be used when rows are added.
        /// </summary>
        public UITableViewRowAnimation AddAnimation { get; set; }

        /// <summary>
        ///     Gets or sets a delegate to a method taking a <see cref="T:UIKit.UITableViewCell" />
        ///     and setting its elements' properties according to the item
        ///     passed as second parameter.
        /// </summary>
        public Action<UITableViewCell, TItem, NSIndexPath> BindCellDelegate { get; set; }

        /// <summary>
        ///     Gets or sets a delegate to a method <see cref="CanEditRow" /> of <see cref="T:UIKit.UITableViewSource" />.
        ///     This method determines whether a row can be edited in a UITableView.
        /// </summary>
        /// <value>The can edit cell delegate. </value>
        public Func<TItem, NSIndexPath, bool> CanEditCellDelegate { get; set; }

        /// <summary>
        ///     Gets or sets a delegate to a method creating or reusing a <see cref="T:UIKit.UITableViewCell" />.
        ///     The cell will then be passed to the <see cref="BindCellDelegate" />
        ///     delegate to set the elements' properties. Note that this delegate is only
        ///     used if you didn't register with a ReuseID using the UITableView.RegisterClassForCell method.
        /// </summary>
        public Func<NSString, UITableViewCell> CreateCellDelegate { get; set; }

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
        ///     Gets or sets which animation should be used when a row is deleted.
        /// </summary>
        public UITableViewRowAnimation DeleteAnimation { get; set; }

        /// <summary>
        ///     Gets or sets the height of the view that will be used for the TableView's footer.
        /// </summary>
        /// <seealso cref="GetViewForFooterDelegate" />
        public Func<nfloat> GetHeightForFooterDelegate { get; set; }

        /// <summary>
        ///     Gets or sets the function, that returns the height of the view that will be used for the TableView's header.
        /// </summary>
        /// <seealso cref="GetViewForHeaderDelegate" />
        public Func<nfloat> GetHeightForHeaderDelegate { get; set; }

        /// <summary>
        ///     Gets or sets the view that will be used as the TableView's footer.
        /// </summary>
        /// <seealso cref="GetHeightForFooterDelegate" />
        public Func<UIView> GetViewForFooterDelegate { get; set; }

        /// <summary>
        ///     Gets or sets the view that will be used as the TableView's header.
        /// </summary>
        /// <seealso cref="GetHeightForHeaderDelegate" />
        public Func<UIView> GetViewForHeaderDelegate { get; set; }

        /// <summary>
        ///     Gets or sets a reuse identifier for the TableView's cells.
        /// </summary>
        public string ReuseId
        {
            get => NsReuseId.ToString();

            set => _reuseId = string.IsNullOrEmpty(value) ? null : new NSString(value);
        }

        /// <summary>
        ///     Gets or sets the UITableView's selected item. You can use one-way databinding on this property.
        /// </summary>
        public TItem SelectedItem
        {
            get => _selectedItem;
            protected set
            {
                _selectedItem = value;
                RaisePropertyChanged(SelectedItemPropertyName);
                RaiseSelectionChanged();
            }
        }

        protected NSString NsReuseId => _reuseId ?? new NSString(DefaultReuseId);

        /// <summary>
        ///     Occurs when a property of this instance changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        ///     Occurs when the last item gets requested in the list.
        /// </summary>
        public event EventHandler LastItemRequested;

        /// <summary>
        ///     Occurs when a new item gets selected in the list.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        ///     Called when item was selected
        /// </summary>
        public event EventHandler<GenericEventArgs<TItem>> ItemSelected;

        /// <summary>
        ///     Called every time when user clicked by item (select/deselect)
        /// </summary>
        public event EventHandler<GenericEventArgs<TItem>> ItemTapped;

        public void SelectItem(TItem item)
        {
            if (_dataSource == null || _view == null)
            {
                return;
            }

            var index = _dataSource.IndexOf(item);
            if (index == -1)
            {
                return;
            }

            var indexPath = NSIndexPath.FromRowSection(index, 0);

            _view.SelectRow(indexPath, false, UITableViewScrollPosition.None);
            RowSelected(_view, indexPath);
        }

        /// <summary>
        ///     Creates and returns a cell for the UITableView. Where needed, this method will
        ///     optimize the reuse of cells for a better performance.
        /// </summary>
        /// <param name="tableView">The UITableView associated to this source.</param>
        /// <param name="indexPath">The NSIndexPath pointing to the item for which the cell must be returned.</param>
        /// <returns>The created and initialised <see cref="T:UIKit.UITableViewCell" />.</returns>
        public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
        {
            if (indexPath.Row + 1 == _dataSource.Count)
            {
                LastItemRequested?.Invoke(this, EventArgs.Empty);
            }

            if (_view == null)
            {
                _view = tableView;
            }

            return GetItemCell(tableView, indexPath);
        }

        protected virtual UITableViewCell GetItemCell(UITableView view, NSIndexPath indexPath)
        {
            var cell = view.DequeueReusableCell(NsReuseId) ?? CreateCell(NsReuseId);

            try
            {
                var coll = _dataSource;

                if (coll != null)
                {
                    var item = coll[indexPath.Row];
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
        ///     When called, checks if the <see cref="GetHeightForFooterDelegate" />has been set.
        ///     If yes, calls that delegate to get the TableView's footer height.
        /// </summary>
        /// <param name="tableView">The active TableView.</param>
        /// <param name="section">The section index.</param>
        /// <returns>The footer's height.</returns>
        /// <remarks>In the current implementation, only one section is supported.</remarks>
        public override nfloat GetHeightForFooter(UITableView tableView, nint section)
        {
            if (GetHeightForFooterDelegate != null)
            {
                return GetHeightForFooterDelegate();
            }

            return 0;
        }

        /// <summary>
        ///     When called, checks if the <see cref="GetHeightForHeaderDelegate" />
        ///     delegate has been set. If yes, calls that delegate to get the TableView's header height.
        /// </summary>
        /// <param name="tableView">The active TableView.</param>
        /// <param name="section">The section index.</param>
        /// <returns>The header's height.</returns>
        /// <remarks>In the current implementation, only one section is supported.</remarks>
        public override nfloat GetHeightForHeader(UITableView tableView, nint section)
        {
            if (GetHeightForHeaderDelegate != null)
            {
                return GetHeightForHeaderDelegate();
            }

            return 0;
        }

        /// <summary>
        ///     Gets the item selected by the NSIndexPath passed as parameter.
        /// </summary>
        /// <param name="indexPath">The NSIndexPath pointing to the desired item.</param>
        /// <returns>The item selected by the NSIndexPath passed as parameter.</returns>
        protected virtual TItem GetItem(NSIndexPath indexPath)
        {
            return _dataSource[indexPath.Row];
        }

        /// <summary>
        ///     When called, checks if the <see cref="GetViewForFooterDelegate" />
        ///     delegate has been set. If yes, calls that delegate to get the TableView's footer.
        /// </summary>
        /// <param name="tableView">The active TableView.</param>
        /// <param name="section">The section index.</param>
        /// <returns>The UIView that should appear as the section's footer.</returns>
        /// <remarks>In the current implementation, only one section is supported.</remarks>
        public override UIView GetViewForFooter(UITableView tableView, nint section)
        {
            if (GetViewForFooterDelegate != null)
            {
                return GetViewForFooterDelegate();
            }

            return base.GetViewForFooter(tableView, section);
        }

        /// <summary>
        ///     When called, checks if the <see cref="GetViewForHeaderDelegate" />
        ///     delegate has been set. If yes, calls that delegate to get the TableView's header.
        /// </summary>
        /// <param name="tableView">The active TableView.</param>
        /// <param name="section">The section index.</param>
        /// <returns>The UIView that should appear as the section's header.</returns>
        /// <remarks>In the current implementation, only one section is supported.</remarks>
        public override UIView GetViewForHeader(UITableView tableView, nint section)
        {
            if (GetViewForHeaderDelegate != null)
            {
                return GetViewForHeaderDelegate();
            }

            return base.GetViewForHeader(tableView, section);
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UITableViewSource.NumberOfSections" /> method.
        /// </summary>
        /// <param name="tableView">The active TableView.</param>
        /// <returns>The number of sections of the UITableView.</returns>
        /// <remarks>In the current implementation, only one section is supported.</remarks>
        public override nint NumberOfSections(UITableView tableView)
        {
            return 1;
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UITableViewSource.RowDeselected" /> method. When called, sets the
        ///     <see cref="SelectedItem" /> property to null and raises the PropertyChanged and the SelectionChanged events.
        /// </summary>
        /// <param name="tableView">The active TableView.</param>
        /// <param name="indexPath">The row's NSIndexPath.</param>
        public override void RowDeselected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = _dataSource != null ? _dataSource[indexPath.Row] : default;
            SelectedItem = default;
            ItemTapped?.Invoke(this, new GenericEventArgs<TItem>(item));
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UITableViewSource.RowSelected" /> method. When called, sets the
        ///     <see cref="SelectedItem" /> property and raises the PropertyChanged and the SelectionChanged events.
        /// </summary>
        /// <param name="tableView">The active TableView.</param>
        /// <param name="indexPath">The row's NSIndexPath.</param>
        public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
        {
            var item = _dataSource != null ? _dataSource[indexPath.Row] : default;
            SelectedItem = item;
            ItemSelected?.Invoke(this, new GenericEventArgs<TItem>(item));
            ItemTapped?.Invoke(this, new GenericEventArgs<TItem>(item));
        }

        /// <summary>
        ///     Overrides the <see cref="M:UIKit.UITableViewSource.RowsInSection" /> method
        ///     and returns the number of rows in the associated data source.
        /// </summary>
        /// <param name="tableview">The active TableView.</param>
        /// <param name="section">The active section.</param>
        /// <returns>The number of rows in the data source.</returns>
        /// <remarks>In the current implementation, only one section is supported.</remarks>
        public override nint RowsInSection(UITableView tableview, nint section)
        {
            if (_view == null)
            {
                _view = tableview;
            }

            return _dataSource == null ? 0 : _dataSource.Count;
        }

        /// <summary>
        ///     Binds a <see cref="T:UIKit.UITableViewCell" /> to an item's properties.
        ///     If a <see cref="BindCellDelegate" /> is available, this delegate will be used.
        ///     If not, a simple text will be shown.
        /// </summary>
        /// <param name="cell">The cell that will be prepared.</param>
        /// <param name="item">The item that should be used to set the cell up.</param>
        /// <param name="indexPath">The <see cref="T:Foundation.NSIndexPath" /> for this cell.</param>
        protected virtual void BindCell(UITableViewCell cell, object item, NSIndexPath indexPath)
        {
            if (BindCellDelegate == null)
            {
                cell.TextLabel.Text = item.ToString();
            }
            else
            {
                BindCellDelegate(cell, (TItem) item, indexPath);
            }
        }

        /// <summary>
        ///     Creates a <see cref="T:UIKit.UITableViewCell" /> corresponding to the reuseId.
        ///     If it is set, the <see cref="CreateCellDelegate" /> delegate will be used.
        /// </summary>
        /// <param name="reuseId">A reuse identifier for the cell.</param>
        /// <returns>The created cell.</returns>
        protected virtual UITableViewCell CreateCell(NSString reuseId)
        {
            if (CreateCellDelegate == null)
            {
                return new UITableViewCell(UITableViewCellStyle.Default, reuseId);
            }

            return CreateCellDelegate(reuseId);
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

                            _view.InsertRows(paths, AddAnimation);
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
                                    SelectedItem = default;
                                }
                            }

                            _view.DeleteRows(paths, DeleteAnimation);
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
                                _view.MoveRow(
                                    NSIndexPath.FromRowSection(e.OldStartingIndex, 0),
                                    NSIndexPath.FromRowSection(e.NewStartingIndex, 0));
                            }
                        }

                        break;

                    default:
                        _view.ReloadData();
                        break;
                }
            });
        }

        /// <summary>
        ///     Cans the edit row.
        /// </summary>
        /// <returns><c>true</c>, if edit row was caned, <c>false</c> otherwise. Returns <c>true</c> by default.</returns>
        /// <param name="tableView">Table view.</param>
        /// <param name="indexPath">Index path.</param>
        public override bool CanEditRow(UITableView tableView, NSIndexPath indexPath)
        {
            if (CanEditCellDelegate != null)
            {
                var collection = _dataSource;

                if (collection != null)
                {
                    var item = collection[indexPath.Row];

                    return CanEditCellDelegate.Invoke(item, indexPath);
                }
            }

            return true;
        }

        private void RaiseSelectionChanged()
        {
            SelectionChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
