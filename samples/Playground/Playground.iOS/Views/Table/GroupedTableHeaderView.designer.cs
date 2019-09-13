// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.iOS.Views.Table
{
	partial class GroupedTableHeaderView
	{
		[Outlet]
		UIKit.UIView ContainerView { get; set; }

		[Outlet]
		UIKit.UIButton InfoButton { get; set; }

		[Outlet]
		UIKit.UILabel TitleLabel { get; set; }

		[Action ("AddButtonAction:")]
		partial void AddButtonAction (Foundation.NSObject sender);

		[Action ("GenerateButtonAction:")]
		partial void GenerateButtonAction (Foundation.NSObject sender);

		[Action ("InfoButtonAction:")]
		partial void InfoButtonAction (Foundation.NSObject sender);
		
		void ReleaseDesignerOutlets ()
		{
			if (ContainerView != null) {
				ContainerView.Dispose ();
				ContainerView = null;
			}

			if (InfoButton != null) {
				InfoButton.Dispose ();
				InfoButton = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
