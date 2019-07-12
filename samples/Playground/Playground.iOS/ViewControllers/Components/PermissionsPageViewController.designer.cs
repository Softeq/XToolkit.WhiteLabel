// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//

using System.CodeDom.Compiler;
using Foundation;

namespace Playground.iOS.ViewControllers.Components
{
    [Register ("PermissionsPageViewController")]
    partial class PermissionsPageViewController
    {
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
        }
    }
}