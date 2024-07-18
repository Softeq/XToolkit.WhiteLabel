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
	[Register ("PermissionsPageViewController")]
	partial class PermissionsPageViewController
	{
		[Outlet]
		UIKit.UIButton Bluetooth { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton Camera { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton LocationAlways { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton LocationInUse { get; set; }

		[Outlet]
		UIKit.UIButton Notifications { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIKit.UIButton Photos { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Camera != null) {
				Camera.Dispose ();
				Camera = null;
			}

			if (LocationAlways != null) {
				LocationAlways.Dispose ();
				LocationAlways = null;
			}

			if (LocationInUse != null) {
				LocationInUse.Dispose ();
				LocationInUse = null;
			}

			if (Photos != null) {
				Photos.Dispose ();
				Photos = null;
			}

			if (Notifications != null) {
				Notifications.Dispose ();
				Notifications = null;
			}

			if (Bluetooth != null) {
				Bluetooth.Dispose ();
				Bluetooth = null;
			}
		}
	}
}
