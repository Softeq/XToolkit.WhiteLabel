// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using Softeq.XToolkit.Bindings.iOS.Bindable.CollectionView;
using System;
using System.CodeDom.Compiler;

namespace Playground.iOS.ViewControllers
{
    [Register ("CollectionViewController")]
    partial class CollectionViewController
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