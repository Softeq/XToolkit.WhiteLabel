// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace RemoteApp.iOS
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		UIKit.UIButton ClearLogBtn { get; set; }

		[Outlet]
		UIKit.UIActivityIndicatorView IndicatorView { get; set; }

		[Outlet]
		UIKit.UITextView LogView { get; set; }

		[Outlet]
		UIKit.UIButton RequestBtn { get; set; }

		[Outlet]
		UIKit.UILabel ResultLabel { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (IndicatorView != null) {
				IndicatorView.Dispose ();
				IndicatorView = null;
			}

			if (LogView != null) {
				LogView.Dispose ();
				LogView = null;
			}

			if (RequestBtn != null) {
				RequestBtn.Dispose ();
				RequestBtn = null;
			}

			if (ResultLabel != null) {
				ResultLabel.Dispose ();
				ResultLabel = null;
			}

			if (ClearLogBtn != null) {
				ClearLogBtn.Dispose ();
				ClearLogBtn = null;
			}
		}
	}
}
