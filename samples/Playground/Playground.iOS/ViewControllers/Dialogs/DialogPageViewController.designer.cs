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
	[Register ("DialogPageViewController")]
	partial class DialogPageViewController
	{
		[Outlet]
		UIKit.UIButton CloseButton { get; set; }

		[Outlet]
		UIKit.UIButton DoneButton { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (CloseButton != null) {
				CloseButton.Dispose ();
				CloseButton = null;
			}

			if (DoneButton != null) {
				DoneButton.Dispose ();
				DoneButton = null;
			}
		}
	}
}
