// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace Playground.iOS.ViewControllers.Components
{
    [Register("ConnectivityPageViewController")]
    partial class ConnectivityPageViewController
    {
        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel ConnectionStatusLabel { get; set; }

        [Outlet]
        [GeneratedCode("iOS Designer", "1.0")]
        UIKit.UILabel ConnectionTypeLabel { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (ConnectionStatusLabel != null)
            {
                ConnectionStatusLabel.Dispose();
                ConnectionStatusLabel = null;
            }

            if (ConnectionTypeLabel != null)
            {
                ConnectionTypeLabel.Dispose();
                ConnectionTypeLabel = null;
            }
        }
    }
}
