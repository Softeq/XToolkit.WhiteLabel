// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreAnimation;
using Foundation;
using ObjCRuntime;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    [Register("GradientView")]
    public class GradientView : UIView
    {
        public GradientView(IntPtr handle) : base(handle)
        {
        }

        public CAGradientLayer GradientLayer => (CAGradientLayer) Layer;

        [Export("layerClass")]
        private static Class MyLayerClass()
        {
            return new Class(typeof(CAGradientLayer));
        }
    }
}