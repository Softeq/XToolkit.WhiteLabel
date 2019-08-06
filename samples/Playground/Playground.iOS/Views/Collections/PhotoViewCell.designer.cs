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
	[Register ("PhotoViewCell")]
	partial class PhotoViewCell
	{
		[Outlet]
		UIKit.UILabel NameLabel { get; set; }

		[Outlet]
		UIKit.UIImageView PhotoImage { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (PhotoImage != null) {
				PhotoImage.Dispose ();
				PhotoImage = null;
			}

			if (NameLabel != null) {
				NameLabel.Dispose ();
				NameLabel = null;
			}
		}
	}
}
