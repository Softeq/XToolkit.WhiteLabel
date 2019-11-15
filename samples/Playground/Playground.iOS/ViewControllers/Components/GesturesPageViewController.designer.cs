// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace Playground.iOS.ViewControllers.Components
{
	[Register ("GesturesPageViewController")]
	partial class GesturesPageViewController
	{
		[Outlet]
		UIKit.UILabel PanLabel { get; set; }

		[Outlet]
		UIKit.UIView PanViewContainer { get; set; }

		[Outlet]
		UIKit.UILabel SwipeLabel { get; set; }

		[Outlet]
		UIKit.UIView SwipeViewContainer { get; set; }

		[Outlet]
		UIKit.UILabel TapLabel { get; set; }

		[Outlet]
		UIKit.UIView TapViewContainer { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (TapViewContainer != null) {
				TapViewContainer.Dispose ();
				TapViewContainer = null;
			}

			if (SwipeViewContainer != null) {
				SwipeViewContainer.Dispose ();
				SwipeViewContainer = null;
			}

			if (PanViewContainer != null) {
				PanViewContainer.Dispose ();
				PanViewContainer = null;
			}

			if (TapLabel != null) {
				TapLabel.Dispose ();
				TapLabel = null;
			}

			if (SwipeLabel != null) {
				SwipeLabel.Dispose ();
				SwipeLabel = null;
			}

			if (PanLabel != null) {
				PanLabel.Dispose ();
				PanLabel = null;
			}
		}
	}
}
