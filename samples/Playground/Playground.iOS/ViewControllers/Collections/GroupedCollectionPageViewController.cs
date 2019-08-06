// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.iOS.Views.Collections;
using Playground.Models;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.WhiteLabel.iOS;

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

            CollectionView.RegisterNibForCell(PhotoViewCell.Nib, PhotoViewCell.Key);
            CollectionView.Delegate = new GroupedCollectionViewDelegateFlowLayout();
            CollectionView.DataSource = new BindableCollectionViewSource<ItemViewModel, PhotoViewCell>(ViewModel.ItemModels);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();
        }
    }
}
