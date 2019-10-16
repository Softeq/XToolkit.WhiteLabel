// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;

namespace Playground.iOS.ViewControllers.Controls
{
    [Register ("PhotoBrowserViewController")]
    partial class PhotoBrowserViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton CameraButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton GalleryButton { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView ImageView { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (CameraButton != null) {
                CameraButton.Dispose ();
                CameraButton = null;
            }

            if (GalleryButton != null) {
                GalleryButton.Dispose ();
                GalleryButton = null;
            }

            if (ImageView != null) {
                ImageView.Dispose ();
                ImageView = null;
            }
        }
    }
}