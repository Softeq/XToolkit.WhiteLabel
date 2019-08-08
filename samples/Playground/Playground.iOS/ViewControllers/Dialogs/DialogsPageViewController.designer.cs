// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.iOS.ViewControllers.Dialogs
{
	[Register ("DialogsPageViewController")]
	partial class DialogsPageViewController
	{
		[Outlet]
		UIKit.UILabel AlertResultLabel { get; set; }

		[Outlet]
		UIKit.UILabel DialogUntilDismissResultLabel { get; set; }

		[Outlet]
		UIKit.UIButton OpenTwoDialogsButton { get; set; }

		[Outlet]
		UIKit.UIButton ShowAlertButton { get; set; }

		[Outlet]
		UIKit.UIButton ShowDialogUntilDismissButton { get; set; }

		[Outlet]
		UIKit.UIButton ShowDialogUntilResultButton { get; set; }

		[Outlet]
		UIKit.UILabel ShowDialogUntilResultLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (AlertResultLabel != null) {
				AlertResultLabel.Dispose ();
				AlertResultLabel = null;
			}

			if (DialogUntilDismissResultLabel != null) {
				DialogUntilDismissResultLabel.Dispose ();
				DialogUntilDismissResultLabel = null;
			}

			if (ShowAlertButton != null) {
				ShowAlertButton.Dispose ();
				ShowAlertButton = null;
			}

			if (ShowDialogUntilDismissButton != null) {
				ShowDialogUntilDismissButton.Dispose ();
				ShowDialogUntilDismissButton = null;
			}

			if (ShowDialogUntilResultButton != null) {
				ShowDialogUntilResultButton.Dispose ();
				ShowDialogUntilResultButton = null;
			}

			if (ShowDialogUntilResultLabel != null) {
				ShowDialogUntilResultLabel.Dispose ();
				ShowDialogUntilResultLabel = null;
			}

			if (OpenTwoDialogsButton != null) {
				OpenTwoDialogsButton.Dispose ();
				OpenTwoDialogsButton = null;
			}
		}
	}
}
