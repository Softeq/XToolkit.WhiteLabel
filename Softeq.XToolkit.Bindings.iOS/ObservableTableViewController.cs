// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    /// <summary>
    ///     A <see cref="UITableViewController" /> that can be used as an iOS view. After setting
    ///     the <see cref="DataSource" /> and the <see cref="BindCellDelegate" /> and <see cref="CreateCellDelegate" />
    ///     properties, the controller can be loaded. If the DataSource is an <see cref="INotifyCollectionChanged" />,
    ///     changes to the collection will be observed and the UI will automatically be updated.
    /// </summary>
    /// <remarks>
    ///     Credits go to Frank A Krueger for the initial idea and the inspiration
    ///     for this class. Frank gracefully accepted to let me add his code (with a few changes)
    ///     to MVVM Light.
    ///     <para>https://gist.github.com/praeclarum/10024108</para>
    /// </remarks>
    /// <typeparam name="T">The type of the items contained in the <see cref="DataSource" />.</typeparam>
    public class ObservableTableViewController<T> : UITableViewController, INotifyPropertyChanged
    {
        /// <summary>
        ///     The <see cref="SelectedItem" /> property's name.
        /// </summary>
        public const string SelectedItemPropertyName = "SelectedItem";

        private IList<T> _dataSource;
        private bool _loadedView;
        private Thread _mainThread;
        private INotifyCollectionChanged _notifier;
        private ObservableTableSource<T> _tableSource;

        /// <summary>
        ///     Initializes a new instance of this class with a plain style.
        /// </summary>
        public ObservableTableViewController()
            : base(UITableViewStyle.Plain)
        {
            Initialize();
        }

        /// <summary>
        ///     Initializes a new instance of this class with a specific style.
        /// </summary>
        /// <param name="tableStyle">The style that will be used for this controller.</param>
        public ObservableTableViewController(UITableViewStyle tableStyle)
            : base(tableStyle)
        {
            Initialize();
        }

        /// <summary>
        ///     When set, specifies which animation should be used when rows change.
        /// </summary>
        public UITableViewRowAnimation AddAnimation { get; set; }

        /// <summary>
        ///     A delegate to a method taking a <see cref="UITableViewCell" />
        ///     and setting its elements' properties according to the item
        ///     passed as second parameter.
        ///     The cell must be created first in the <see cref="CreateCellDelegate" />
        ///     delegate.
        /// </summary>
        public Action<UITableViewCell, T, NSIndexPath> BindCellDelegate { get; set; }

        /// <summary>
        ///     A delegate to a method creating or reusing a <see cref="UITableViewCell" />.
        ///     The cell will then be passed to the <see cref="BindCellDelegate" />
        ///     delegate to set the elements' properties.
        /// </summary>
        public Func<T, UITableView, UITableViewCell> CreateCellDelegate { get; set; }

        /// <summary>
        ///     The data source of this list controller.
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
                    _notifier.CollectionChanged -= HandleCollectionChanged;
                }

                _dataSource = value;
                _notifier = value as INotifyCollectionChanged;

                if (_notifier != null)
                {
                    _notifier.CollectionChanged += HandleCollectionChanged;
                }

                if (_loadedView)
                {
                    TableView.ReloadData();
                }
            }
        }

        /// <summary>
        ///     When set, specifieds which animation should be used when a row is deleted.
        /// </summary>
        public UITableViewRowAnimation DeleteAnimation { get; set; }

        /// <summary>
        ///     When set, returns the height of the view that will be used for the TableView's footer.
        /// </summary>
        /// <seealso cref="GetViewForFooterDelegate" />
        public Func<UITableView, nint, nfloat> GetHeightForFooterDelegate { get; set; }

        /// <summary>
        ///     When set, returns the height of the view that will be used for the TableView's header.
        /// </summary>
        /// <seealso cref="GetViewForHeaderDelegate" />
        public Func<UITableView, nint, nfloat> GetHeightForHeaderDelegate { get; set; }

        /// <summary>
        ///     When set, returns a view that can be used as the TableView's footer.
        /// </summary>
        /// <seealso cref="GetHeightForFooterDelegate" />
        public Func<UITableView, nint, UIView> GetViewForFooterDelegate { get; set; }

        /// <summary>
        ///     When set, returns a view that can be used as the TableView's header.
        /// </summary>
        /// <seealso cref="GetHeightForHeaderDelegate" />
        public Func<UITableView, nint, UIView> GetViewForHeaderDelegate { get; set; }

        public Action<NSIndexPath, int, int> ScrollAfterInsertDelegate { get; set; }

        public Action DraggingStarted { get; set; }

        /// <summary>
        ///     A reuse identifier for the TableView's cells.
        /// </summary>
        public string ReuseId { get; set; }

        /// <summary>
        ///     Gets the TableView's selected item.
        ///     Changes to that property's value raise the PropertyChanged event.
        /// </summary>
        public T SelectedItem { get; private set; }

        /// <summary>
        ///     The source of the TableView.
        /// </summary>
        public UITableViewSource TableSource => _tableSource;

        /// <summary>
        ///     Overrides <see cref="UITableViewController.TableView" />.
        ///     Sets or gets the controllers TableView. If you use a TableView
        ///     placed in the UI manually, use this property's setter to assign
        ///     your TableView to this controller.
        /// </summary>
        public override UITableView TableView
        {
            get => base.TableView;
            set
            {
                base.TableView = value;
                base.TableView.Source = _tableSource ?? CreateSource();
                _loadedView = true;
            }
        }

        /// <summary>
        ///     Occurs when a property of this instance changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler LastItemRequested;

        /// <summary>
        ///     Overrides the <see cref="UIViewController.ViewDidLoad" /> method.
        /// </summary>
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            TableView.Source = CreateSource();
            _loadedView = true;
        }

        /// <summary>
        ///     Binds a <see cref="UITableViewCell" /> to an item's properties.
        ///     If a <see cref="BindCellDelegate" /> is available, this delegate will be used.
        ///     If not, a simple text will be shown.
        /// </summary>
        /// <param name="cell">The cell that will be prepared.</param>
        /// <param name="item">The item that should be used to set the cell up.</param>
        /// <param name="indexPath">The <see cref="NSIndexPath" /> for this cell.</param>
        protected virtual void BindCell(UITableViewCell cell, object item, NSIndexPath indexPath)
        {
            if (BindCellDelegate == null)
            {
                cell.TextLabel.Text = item.ToString();
            }
            else
            {
                BindCellDelegate(cell, (T) item, indexPath);
            }
        }

        /// <summary>
        ///     Creates a <see cref="UITableViewCell" /> corresponding to the reuseId.
        ///     If it is set, the <see cref="CreateCellDelegate" /> delegate will be used.
        /// </summary>
        /// <param name="reuseId">A reuse identifier for the cell.</param>
        /// <returns>The created cell.</returns>
        protected virtual UITableViewCell CreateCell(T model)
        {
            if (CreateCellDelegate == null
                || BindCellDelegate == null)
            {
                return new UITableViewCell(UITableViewCellStyle.Default, ReuseId);
            }

            return CreateCellDelegate(model, TableView);
        }

        /// <summary>
        ///     Created the ObservableTableSource for this controller.
        /// </summary>
        /// <returns>The created ObservableTableSource.</returns>
        protected virtual ObservableTableSource<T> CreateSource()
        {
            _tableSource = new ObservableTableSource<T>(this)
            {
                ReuseId = ReuseId
            };

            _tableSource.LastItemRequested += OnLastItemRequested;
            _tableSource.OnDraggingStarted += OnDraggingStarted;

            return _tableSource;
        }

        /// <summary>
        ///     Called when a row gets selected. Raises the SelectionChanged event.
        /// </summary>
        /// <param name="item">The selected item.</param>
        /// <param name="indexPath">The NSIndexPath for the selected row.</param>
        protected virtual void OnRowSelected(object item, NSIndexPath indexPath)
        {
            SelectedItem = (T) item;

            // ReSharper disable ExplicitCallerInfoArgument
            RaisePropertyChanged(SelectedItemPropertyName);
            // ReSharper restore ExplicitCallerInfoArgument

            var handler = SelectionChanged;
            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        /// <summary>
        ///     Raises the <see cref="PropertyChanged" /> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected virtual void RaisePropertyChanged([CallerMemberName] string propertyName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (!_loadedView)
            {
                return;
            }

            Action act = () =>
            {
                if (e.Action == NotifyCollectionChangedAction.Reset)
                {
                    TableView.ReloadData();
                }

                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    var firstVisibleElement = TableView.IndexPathsForVisibleRows.Any()
                        ? TableView.IndexPathsForVisibleRows[0]
                        : null;
                    TableView.ReloadData();
                    ScrollAfterInsertDelegate?.Invoke(firstVisibleElement, e.NewItems.Count, e.NewStartingIndex);
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    var count = e.OldItems.Count;
                    var paths = new NSIndexPath[count];
                    for (var i = 0; i < count; i++)
                    {
                        paths[i] = NSIndexPath.FromRowSection(e.OldStartingIndex + i, 0);
                    }

                    TableView.DeleteRows(paths, DeleteAnimation);
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

        private void Initialize()
        {
            _mainThread = Thread.CurrentThread;

            AddAnimation = UITableViewRowAnimation.Automatic;
            DeleteAnimation = UITableViewRowAnimation.Automatic;
        }

        private void OnLastItemRequested(object sender, EventArgs e)
        {
            LastItemRequested?.Invoke(this, EventArgs.Empty);
        }

        private void OnDraggingStarted()
        {
            DraggingStarted?.Invoke();
        }

        /// <summary>
        ///     Occurs when a new item gets selected in the list.
        /// </summary>
        public event EventHandler SelectionChanged;

        /// <summary>
        ///     A <see cref="UITableViewSource" /> that handles changes to the underlying
        ///     data source if this data source is an <see cref="INotifyCollectionChanged" />.
        /// </summary>
        /// <typeparam name="T2">The type of the items that the data source contains.</typeparam>
        /// <remarks>In the current implementation, only one section is supported.</remarks>
        protected class ObservableTableSource<T2> : UITableViewSource
        {
            private readonly ObservableTableViewController<T2> _controller;
            private readonly NSString _defaultReuseId = new NSString("C");

            private NSString _reuseId;

            public EventHandler LastItemRequested;
            public Action OnDraggingStarted;

            /// <summary>
            ///     Initializes an instance of this class.
            /// </summary>
            /// <param name="controller">The controller associated to this instance.</param>
            public ObservableTableSource(ObservableTableViewController<T2> controller)
            {
                _controller = controller;
            }

            /// <summary>
            ///     A reuse identifier for the TableView's cells.
            /// </summary>
            public string ReuseId
            {
                get => NsReuseId.ToString();

                set => _reuseId = string.IsNullOrEmpty(value) ? null : new NSString(value);
            }

            private NSString NsReuseId => _reuseId ?? _defaultReuseId;

            /// <summary>
            ///     Attempts to dequeue or create a cell for the list.
            /// </summary>
            /// <param name="tableView">The TableView that is the cell's parent.</param>
            /// <param name="indexPath">The NSIndexPath for the cell.</param>
            /// <returns>The created or recycled cell.</returns>
            public override UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
            {
                if (indexPath.Row + 1 == _controller._dataSource.Count)
                {
                    LastItemRequested?.Invoke(this, EventArgs.Empty);
                }

                try
                {
                    var coll = _controller._dataSource;

                    if (coll != null)
                    {
                        var obj = coll[indexPath.Row];

                        var cell = tableView.DequeueReusableCell(NsReuseId) ??
                                   _controller.CreateCell(obj);

                        _controller.BindCell(cell, obj, indexPath);
                        return cell;
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex);
                }

                return null;
            }

            /// <summary>
            ///     When called, checks if the ObservableTableViewController{T}.GetHeightForFooter
            ///     delegate has been set. If yes, calls that delegate to get the TableView's footer height.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The section index.</param>
            /// <returns>The footer's height.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
            public override nfloat GetHeightForFooter(UITableView tableView, nint section)
            {
                if (_controller.GetHeightForFooterDelegate != null)
                {
                    return _controller.GetHeightForFooterDelegate(tableView, section);
                }

                return 0;
            }

            /// <summary>
            ///     When called, checks if the ObservableTableViewController{T}.GetHeightForHeader
            ///     delegate has been set. If yes, calls that delegate to get the TableView's header height.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The section index.</param>
            /// <returns>The header's height.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
            public override nfloat GetHeightForHeader(UITableView tableView, nint section)
            {
                if (_controller.GetHeightForHeaderDelegate != null)
                {
                    return _controller.GetHeightForHeaderDelegate(tableView, section);
                }

                return 0;
            }

            /// <summary>
            ///     When called, checks if the ObservableTableViewController{T}.GetViewForFooter
            ///     delegate has been set. If yes, calls that delegate to get the TableView's footer.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The section index.</param>
            /// <returns>The UIView that should appear as the section's footer.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
            public override UIView GetViewForFooter(UITableView tableView, nint section)
            {
                if (_controller.GetViewForFooterDelegate != null)
                {
                    return _controller.GetViewForFooterDelegate(tableView, section);
                }

                return base.GetViewForFooter(tableView, section);
            }

            /// <summary>
            ///     When called, checks if the ObservableTableViewController{T}.GetViewForHeader
            ///     delegate has been set. If yes, calls that delegate to get the TableView's header.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The section index.</param>
            /// <returns>The UIView that should appear as the section's header.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
            public override UIView GetViewForHeader(UITableView tableView, nint section)
            {
                if (_controller.GetViewForHeaderDelegate != null)
                {
                    return _controller.GetViewForHeaderDelegate(tableView, section);
                }

                return base.GetViewForHeader(tableView, section);
            }

            /// <summary>
            ///     Overrides the <see cref="UITableViewSource.NumberOfSections" /> method.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <returns>The number of sections of the UITableView.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
            public override nint NumberOfSections(UITableView tableView)
            {
                return 1;
            }

            /// <summary>
            ///     Overrides the <see cref="UITableViewSource.RowSelected" /> method
            ///     and notifies the associated <see cref="ObservableTableViewController{T}" />
            ///     that a row has been selected, so that the corresponding events can be raised.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="indexPath">The row's NSIndexPath.</param>
            public override void RowSelected(UITableView tableView, NSIndexPath indexPath)
            {
                var item = _controller._dataSource != null ? _controller._dataSource[indexPath.Row] : default(T2);
                _controller.OnRowSelected(item, indexPath);
            }

            /// <summary>
            ///     Overrides the <see cref="UITableViewSource.RowsInSection" /> method
            ///     and returns the number of rows in the associated data source.
            /// </summary>
            /// <param name="tableView">The active TableView.</param>
            /// <param name="section">The active section.</param>
            /// <returns>The number of rows in the data source.</returns>
            /// <remarks>In the current implementation, only one section is supported.</remarks>
            public override nint RowsInSection(UITableView tableView, nint section)
            {
                var coll = _controller._dataSource;
                return coll != null ? coll.Count : 0;
            }

            public override void DraggingStarted(UIScrollView scrollView)
            {
                OnDraggingStarted?.Invoke();
            }
        }
    }
}