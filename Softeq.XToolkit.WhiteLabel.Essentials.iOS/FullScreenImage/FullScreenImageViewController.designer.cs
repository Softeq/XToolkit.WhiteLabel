// WARNING
//
// This file has been generated automatically by Visual Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//

using Foundation;

namespace Softeq.XToolkit.WhiteLabel.Essentials.iOS.FullScreenImage
{
	[Register ("FullScreenImageViewController")]
	partial class FullScreenImageViewController
	{
		[Outlet]
		UIKit.UIButton CloseButton { get; set; }

		[Outlet]
		UIKit.UIImageView ImageView { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (ImageView != null) {
				ImageView.Dispose ();
				ImageView = null;
			}

			if (CloseButton != null) {
				CloseButton.Dispose ();
				CloseButton = null;
			}
		}
	}
}
