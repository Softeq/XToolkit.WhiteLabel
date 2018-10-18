// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public class CustomViewBase : UIView
    {
        public CustomViewBase(IntPtr handle) : base(handle)
        {
            Initialize();
        }

        public CustomViewBase(CGRect frame) : base(frame)
        {
            Initialize();
        }

        protected virtual void DoInit()
        {
        }

        private void Initialize()
        {
            var xibName = GetType().Name;
            var nib = UINib.FromName(xibName, NSBundle.MainBundle);
            var view = nib.Instantiate(this, null)[0] as UIView;
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(view);
            var right = view.RightAnchor.ConstraintEqualTo(RightAnchor);
            var left = view.LeftAnchor.ConstraintEqualTo(LeftAnchor);
            var top = view.TopAnchor.ConstraintEqualTo(TopAnchor);
            var bottom = view.BottomAnchor.ConstraintEqualTo(BottomAnchor);
            NSLayoutConstraint.ActivateConstraints(new[] {right, left, top, bottom});

            DoInit();
        }
    }
}