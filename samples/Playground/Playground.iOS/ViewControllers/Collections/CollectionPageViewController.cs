// Developed by Softeq Development Corporation
// http://www.softeq.com

using ObjCRuntime;
using Playground.iOS.Views;
using Playground.Models;
using Playground.ViewModels.Collections;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Collections
{
    public partial class CollectionPageViewController
        : ViewControllerBase<CollectionPageViewModel>
    {
        public CollectionPageViewController(NativeHandle handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CollectionView.RegisterNibForCell(MovieCollectionViewCell.Nib, MovieCollectionViewCell.Key);
            CollectionView.Delegate = new BindableUICollectionViewDelegateFlowLayout();
            CollectionView.DataSource = new BindableCollectionViewSource<ItemViewModel, MovieCollectionViewCell>(ViewModel.ItemModels)
            {
                ItemClick = ViewModel.SelectItemCommand
            };
        }
    }
}
