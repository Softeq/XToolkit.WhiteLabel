// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Foundation;
using Playground.iOS.Views.Collections;
using Playground.Models;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Collections
{
    public partial class GroupedCollectionPageViewController : ViewControllerBase<GroupedCollectionPageViewModel>
    {
        public GroupedCollectionPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CollectionView.RegisterNibForSupplementaryView(GroupedHeaderView.Nib, UICollectionElementKindSection.Header, GroupedHeaderView.Key);
            CollectionView.RegisterNibForCell(PhotoViewCell.Nib, PhotoViewCell.Key);
            CollectionView.Delegate = new GroupedCollectionViewDelegateFlowLayout();
            CollectionView.DataSource = new BindableGroupedCollectionViewSource<ItemViewModel, PhotoViewCell, string, GroupedHeaderView>(ViewModel.ItemModels);

            // pin headers
            ((UICollectionViewFlowLayout) CollectionView.CollectionViewLayout).SectionHeadersPinToVisibleBounds = true;
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();
        }

        internal class BindableGroupedCollectionViewSource<TItem, TCell, TGroup, THeader>
            : BindableCollectionViewSource<TItem, TCell>
            where TCell : BindableCollectionViewCell<TItem>
            where THeader : BindableUICollectionReusableView<TGroup>
        {
            public BindableGroupedCollectionViewSource(IList<TItem> dataSource) : base(dataSource)
            {
            }

            /// <inheritdoc />
            public override UICollectionReusableView GetViewForSupplementaryElement(
                UICollectionView collectionView, NSString elementKind, NSIndexPath indexPath)
            {
                if (elementKind == UICollectionElementKindSectionKey.Header)
                {

                }
                else if (elementKind == UICollectionElementKindSectionKey.Footer)
                {

                }

                var header = collectionView.DequeueReusableSupplementaryView(elementKind, typeof(THeader).Name, indexPath);

                return header;
            }
        }
    }


    public class BindableUICollectionReusableView<T> : UICollectionReusableView, IBindableView
    {
        public BindableUICollectionReusableView(IntPtr handle) : base(handle)
        {
        }

        public List<Binding> Bindings { get; } = new List<Binding>();

        public object DataContext { get; private set; }

        protected T ViewModel => (T) DataContext;

        public virtual void DoAttachBindings()
        {
        }

        public virtual void DoDetachBindings()
        {
            this.DetachBindings();
        }

        void IBindable.SetDataContext(object dataContext)
        {
            DataContext = dataContext;
        }
    }
}
