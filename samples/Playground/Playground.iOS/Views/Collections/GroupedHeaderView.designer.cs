// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.iOS.Views.Collections
{
    [Register(nameof(GroupedHeaderView))]
    partial class GroupedHeaderView
    {
        [Outlet]
        UIKit.UILabel TitleLabel { get; set; }

        void ReleaseDesignerOutlets()
        {
            if (TitleLabel != null)
            {
                TitleLabel.Dispose();
                TitleLabel = null;
            }
        }
    }
}
