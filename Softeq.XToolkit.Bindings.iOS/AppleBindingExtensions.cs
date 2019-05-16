// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.Bindings.iOS
{
    /// <summary>
    ///     Defines extension methods for iOS only.
    /// </summary>
    public static class AppleBindingExtensions
    {
        /// <summary>
        ///     Creates a new <see cref="ObservableCollectionViewSource{TItem, TCell}" /> for a given <see cref="IList{TItem}" />.
        ///     Note that if the IList doesn't implement INotifyCollectionChanged, the associated UICollectionView won't be
        ///     updated when the IList changes.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the IList.</typeparam>
        /// <typeparam name="TCell">The type of cells in the CollectionView associated to this ObservableCollectionViewSource.</typeparam>
        /// <param name="list">The IList that should be represented in the associated UICollectionView</param>
        /// <param name="bindCellDelegate">
        ///     A delegate to a method taking a <see cref="UICollectionViewCell" />
        ///     and setting its elements' properties according to the item passed as second parameter.
        /// </param>
        /// <param name="getSupplementaryViewDelegate">
        ///     A delegate to a method returning a <see cref="UICollectionReusableView" />
        ///     and used to set supplementary views on the UICollectionView.
        /// </param>
        /// <param name="reuseId">An ID used for optimization and cell reuse.</param>
        /// <param name="factory">
        ///     An optional delegate returning an instance of a class deriving from
        ///     <see cref="ObservableCollectionViewSource{TItem, TCell}" />. This can be used if you need to implement
        ///     specific features in addition to the built-in features of ObservableCollectionViewSource.
        /// </param>
        /// <returns>The new instance of ObservableCollectionViewSource.</returns>
        public static ObservableCollectionViewSource<TItem, TCell> GetCollectionViewSource<TItem, TCell>(
            this IList<TItem> list,
            Action<TCell, TItem, NSIndexPath> bindCellDelegate,
            Func<NSString, NSIndexPath, UICollectionReusableView> getSupplementaryViewDelegate = null,
            string reuseId = null,
            Func<ObservableCollectionViewSource<TItem, TCell>> factory = null)
            where TCell : UICollectionViewCell
        {
            if (factory != null)
            {
                var coll = factory();
                coll.DataSource = list;
                coll.BindCellDelegate = bindCellDelegate;
                coll.GetSupplementaryViewDelegate = getSupplementaryViewDelegate;
                coll.ReuseId = reuseId;
                return coll;
            }

            return new ObservableCollectionViewSource<TItem, TCell>
            {
                DataSource = list,
                BindCellDelegate = bindCellDelegate,
                GetSupplementaryViewDelegate = getSupplementaryViewDelegate,
                ReuseId = reuseId
            };
        }

        /// <summary>
        ///     Creates a new <see cref="ObservableCollectionViewSource{TItem, TCell}" /> for a given
        ///     <see cref="ObservableCollection{TItem}" />.
        ///     The associated UICollectionView will be updated when the ObservableCollection changes.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the IList.</typeparam>
        /// <typeparam name="TCell">The type of cells in the CollectionView associated to this ObservableCollectionViewSource.</typeparam>
        /// <param name="list">The ObservableCollection that should be represented in the associated UICollectionView</param>
        /// <param name="bindCellDelegate">
        ///     A delegate to a method taking a <see cref="UICollectionViewCell" />
        ///     and setting its elements' properties according to the item passed as second parameter.
        /// </param>
        /// <param name="getSupplementaryViewDelegate">
        ///     A delegate to a method returing a <see cref="UICollectionReusableView" />
        ///     and used to set supplementary views on the UICollectionView.
        /// </param>
        /// <param name="reuseId">An ID used for optimization and cell reuse.</param>
        /// <param name="factory">
        ///     An optional delegate returning an instance of a class deriving from
        ///     <see cref="ObservableCollectionViewSource{TItem, TCell}" />. This can be used if you need to implement
        ///     specific features in addition to the built-in features of ObservableCollectionViewSource.
        /// </param>
        /// <returns>The new instance of ObservableCollectionViewSource.</returns>
        public static ObservableCollectionViewSource<TItem, TCell> GetCollectionViewSource<TItem, TCell>(
            this ObservableCollection<TItem> list,
            Action<TCell, TItem, NSIndexPath> bindCellDelegate,
            Func<NSString, NSIndexPath, UICollectionReusableView> getSupplementaryViewDelegate = null,
            string reuseId = null,
            Func<ObservableCollectionViewSource<TItem, TCell>> factory = null)
            where TCell : UICollectionViewCell
        {
            if (factory != null)
            {
                var coll = factory();
                coll.DataSource = list;
                coll.BindCellDelegate = bindCellDelegate;
                coll.GetSupplementaryViewDelegate = getSupplementaryViewDelegate;
                coll.ReuseId = reuseId;
                return coll;
            }

            return new ObservableCollectionViewSource<TItem, TCell>
            {
                DataSource = list,
                BindCellDelegate = bindCellDelegate,
                GetSupplementaryViewDelegate = getSupplementaryViewDelegate,
                ReuseId = reuseId
            };
        }

        /// <summary>
        ///     Creates a new <see cref="ObservableTableViewController{TItem}" /> for a given
        ///     <see cref="ObservableCollection{TItem}" />.
        /// </summary>
        /// <typeparam name="TItem">The type of the items contained in the collection.</typeparam>
        /// <param name="collection">The collection that the adapter will be created for.</param>
        /// <param name="createCellDelegate">
        ///     A delegate to a method creating or reusing a <see cref="UITableViewCell" />.
        ///     The cell will then be passed to the bindCellDelegate delegate to set the elements' properties.
        ///     If you use a reuseId, you can pass null for the createCellDelegate.
        /// </param>
        /// <param name="bindCellDelegate">
        ///     A delegate to a method taking a <see cref="UITableViewCell" />
        ///     and setting its elements' properties according to the item passed as second parameter.
        ///     The cell must be created first in the createCellDelegate delegate, unless a
        ///     reuseId is passed to the method.
        /// </param>
        /// <param name="reuseId">A reuse identifier for the TableView's cells.</param>
        /// <returns>A controller adapted to the collection passed in parameter.</returns>
        public static ObservableTableViewController<TItem> GetController<TItem>(
            this ObservableCollection<TItem> collection,
            Func<TItem, UITableView, UITableViewCell> createCellDelegate,
            Action<UITableViewCell, TItem, NSIndexPath> bindCellDelegate,
            string reuseId = null)
        {
            return new ObservableTableViewController<TItem>
            {
                DataSource = collection,
                CreateCellDelegate = createCellDelegate,
                BindCellDelegate = bindCellDelegate,
                ReuseId = reuseId
            };
        }

        /// <summary>
        ///     Creates a new <see cref="ObservableTableViewController{TItem}" /> for a given <see cref="IList{TItem}" />.
        /// </summary>
        /// <typeparam name="TItem">The type of the items contained in the list.</typeparam>
        /// <param name="list">The list that the adapter will be created for.</param>
        /// <param name="createCellDelegate">
        ///     A delegate to a method creating or reusing a <see cref="UITableViewCell" />.
        ///     The cell will then be passed to the bindCellDelegate delegate to set the elements' properties.
        ///     If you use a reuseId, you can pass null for the createCellDelegate.
        /// </param>
        /// <param name="bindCellDelegate">
        ///     A delegate to a method taking a <see cref="UITableViewCell" />
        ///     and setting its elements' properties according to the item passed as second parameter.
        ///     The cell must be created first in the createCellDelegate delegate, unless a reuseId is
        ///     passed to the method.
        /// </param>
        /// <param name="reuseId">A reuse identifier for the TableView's cells.</param>
        /// <returns>A controller adapted to the collection passed in parameter.</returns>
        public static ObservableTableViewController<TItem> GetController<TItem>(
            this IList<TItem> list,
            Func<TItem, UITableView, UITableViewCell> createCellDelegate,
            Action<UITableViewCell, TItem, NSIndexPath> bindCellDelegate,
            string reuseId = null)
        {
            return new ObservableTableViewController<TItem>
            {
                DataSource = list,
                CreateCellDelegate = createCellDelegate,
                BindCellDelegate = bindCellDelegate,
                ReuseId = reuseId
            };
        }

        /// <summary>
        ///     Creates a new <see cref="ObservableTableViewSource{TItem}" /> for a given <see cref="IList{TItem}" />.
        ///     Note that if the IList doesn't implement INotifyCollectionChanged, the associated UITableView won't be
        ///     updated when the IList changes.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the IList.</typeparam>
        /// <param name="list">The IList that should be represented in the associated UITableView</param>
        /// <param name="bindCellDelegate">
        ///     A delegate to a method taking a <see cref="UITableViewCell" />
        ///     and setting its elements' properties according to the item passed as second parameter.
        /// </param>
        /// <param name="reuseId">An ID used for optimization and cell reuse.</param>
        /// <param name="factory">
        ///     An optional delegate returning an instance of a class deriving from
        ///     <see cref="ObservableTableViewSource{TItem}" />. This can be used if you need to implement
        ///     specific features in addition to the built-in features of ObservableTableViewSource.
        /// </param>
        /// <returns>The new instance of ObservableTableViewSource.</returns>
        public static ObservableTableViewSource<TItem> GetTableViewSource<TItem>(
            this IList<TItem> list,
            Action<UITableViewCell, TItem, NSIndexPath> bindCellDelegate,
            string reuseId = null,
            Func<ObservableTableViewSource<TItem>> factory = null)
        {
            if (factory != null)
            {
                var coll = factory();
                coll.DataSource = list;
                coll.BindCellDelegate = bindCellDelegate;
                coll.ReuseId = reuseId;
                return coll;
            }

            return new ObservableTableViewSource<TItem>
            {
                DataSource = list,
                BindCellDelegate = bindCellDelegate,
                ReuseId = reuseId
            };
        }

        /// <summary>
        ///     Creates a new <see cref="ObservableTableViewSource{TItem}" /> for a given
        ///     <see cref="ObservableCollection{TItem}" />.
        ///     The associated UITableView will be updated when the ObservableCollection changes.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the IList.</typeparam>
        /// <param name="list">The ObservableCollection that should be represented in the associated UITableView</param>
        /// <param name="bindCellDelegate">
        ///     A delegate to a method taking a <see cref="UITableViewCell" />
        ///     and setting its elements' properties according to the item passed as second parameter.
        /// </param>
        /// <param name="reuseId">An ID used for optimization and cell reuse.</param>
        /// <param name="factory">
        ///     An optional delegate returning an instance of a class deriving from
        ///     <see cref="ObservableTableViewSource{TItem}" />. This can be used if you need to implement
        ///     specific features in addition to the built-in features of ObservableTableViewSource.
        /// </param>
        /// <returns>The new instance of ObservableTableViewSource.</returns>
        public static ObservableTableViewSource<TItem> GetTableViewSource<TItem>(
            this ObservableCollection<TItem> list,
            Action<UITableViewCell, TItem, NSIndexPath> bindCellDelegate,
            string reuseId = null,
            Func<ObservableTableViewSource<TItem>> factory = null)
        {
            if (factory != null)
            {
                var coll = factory();
                coll.DataSource = list;
                coll.BindCellDelegate = bindCellDelegate;
                coll.ReuseId = reuseId;
                return coll;
            }

            return new ObservableTableViewSource<TItem>
            {
                DataSource = list,
                BindCellDelegate = bindCellDelegate,
                ReuseId = reuseId
            };
        }

        /// <summary>
        ///     Creates a new <see cref="ObservableTableViewSource{TItem}" /> for a given <see cref="IList{TItem}" />.
        ///     Note that if the IList doesn't implement INotifyCollectionChanged, the associated UITableView won't be
        ///     updated when the IList changes.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the IList.</typeparam>
        /// <param name="list">The IList that should be represented in the associated UITableView</param>
        /// <param name="createCellDelegate">
        ///     A delegate to a method creating or reusing a <see cref="UITableViewCell" />.
        ///     The cell will then be passed to the bindCellDelegate delegate to set the elements' properties.
        ///     Use this method only if you don't want to register with the UITableView.RegisterClassForCellReuse method
        ///     for cell reuse.
        /// </param>
        /// <param name="bindCellDelegate">
        ///     A delegate to a method taking a <see cref="UITableViewCell" />
        ///     and setting its elements' properties according to the item passed as second parameter.
        /// </param>
        /// <param name="reuseId">An ID used for optimization and cell reuse.</param>
        /// <param name="factory">
        ///     An optional delegate returning an instance of a class deriving from
        ///     <see cref="ObservableTableViewSource{TItem}" />. This can be used if you need to implement
        ///     specific features in addition to the built-in features of ObservableTableViewSource.
        /// </param>
        /// <returns>The new instance of ObservableTableViewSource.</returns>
        public static ObservableTableViewSource<TItem> GetTableViewSource<TItem>(
            this IList<TItem> list,
            Func<NSString, UITableViewCell> createCellDelegate,
            Action<UITableViewCell, TItem, NSIndexPath> bindCellDelegate,
            string reuseId = null,
            Func<ObservableTableViewSource<TItem>> factory = null)
        {
            if (factory != null)
            {
                var coll = factory();
                coll.DataSource = list;
                coll.BindCellDelegate = bindCellDelegate;
                coll.CreateCellDelegate = createCellDelegate;
                coll.ReuseId = reuseId;
                return coll;
            }

            return new ObservableTableViewSource<TItem>
            {
                DataSource = list,
                BindCellDelegate = bindCellDelegate,
                CreateCellDelegate = createCellDelegate,
                ReuseId = reuseId
            };
        }

        /// <summary>
        ///     Creates a new <see cref="ObservableTableViewSource{TItem}" /> for a given
        ///     <see cref="ObservableCollection{TItem}" />.
        ///     The associated UITableView will be updated when the ObservableCollection changes.
        /// </summary>
        /// <typeparam name="TItem">The type of the items in the IList.</typeparam>
        /// <param name="list">The ObservableCollection that should be represented in the associated UITableView</param>
        /// <param name="bindCellDelegate">
        ///     A delegate to a method taking a <see cref="UITableViewCell" />
        ///     and setting its elements' properties according to the item passed as second parameter.
        /// </param>
        /// <param name="createCellDelegate">
        ///     A delegate to a method creating or reusing a <see cref="UITableViewCell" />.
        ///     The cell will then be passed to the bindCellDelegate delegate to set the elements' properties.
        ///     Use this method only if you don't want to register with the UITableView.RegisterClassForCellReuse method
        ///     for cell reuse.
        /// </param>
        /// <param name="reuseId">An ID used for optimization and cell reuse.</param>
        /// <param name="factory">
        ///     An optional delegate returning an instance of a class deriving from
        ///     <see cref="ObservableTableViewSource{TItem}" />. This can be used if you need to implement
        ///     specific features in addition to the built-in features of ObservableTableViewSource.
        /// </param>
        /// <returns>The new instance of ObservableTableViewSource.</returns>
        public static ObservableTableViewSource<TItem> GetTableViewSource<TItem>(
            this ObservableCollection<TItem> list,
            Func<NSString, UITableViewCell> createCellDelegate,
            Action<UITableViewCell, TItem, NSIndexPath> bindCellDelegate,
            string reuseId = null,
            Func<ObservableTableViewSource<TItem>> factory = null)
        {
            if (factory != null)
            {
                var coll = factory();
                coll.DataSource = list;
                coll.BindCellDelegate = bindCellDelegate;
                coll.CreateCellDelegate = createCellDelegate;
                coll.ReuseId = reuseId;
                return coll;
            }

            return new ObservableTableViewSource<TItem>
            {
                DataSource = list,
                BindCellDelegate = bindCellDelegate,
                CreateCellDelegate = createCellDelegate,
                ReuseId = reuseId
            };
        }
    }
}