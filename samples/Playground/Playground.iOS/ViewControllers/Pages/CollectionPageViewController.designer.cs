// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using Foundation;
using Softeq.XToolkit.Bindings.iOS.Bindable.CollectionView;

namespace Playground.iOS.ViewControllers.Pages
{
    [Register ("CollectionPageViewController")]
    partial class CollectionPageViewController
    {
        [Outlet]
        BindableCollectionView CollectionView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CollectionView != null) {
                CollectionView.Dispose ();
                CollectionView = null;
            }
        }
    }
}