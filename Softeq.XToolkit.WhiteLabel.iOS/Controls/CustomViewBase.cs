// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Controls
{
    public abstract class CustomViewBase : UIView
    {
        protected CustomViewBase(IntPtr handle) : base(handle)
        {
#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            Initialize();
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor
        }

        protected CustomViewBase(CGRect frame) : base(frame)
        {
#pragma warning disable RECS0021 // Warns about calls to virtual member functions occuring in the constructor
            Initialize();
#pragma warning restore RECS0021 // Warns about calls to virtual member functions occuring in the constructor
        }

        protected virtual string XibName => GetType().Name;

        protected virtual void Initialize()
        {
            var nib = UINib.FromName(XibName, NSBundle.MainBundle);
            var view = nib.Instantiate(this, null)[0] as UIView;
            view.TranslatesAutoresizingMaskIntoConstraints = false;
            AddSubview(view);
            var right = view.RightAnchor.ConstraintEqualTo(RightAnchor);
            var left = view.LeftAnchor.ConstraintEqualTo(LeftAnchor);
            var top = view.TopAnchor.ConstraintEqualTo(TopAnchor);
            var bottom = view.BottomAnchor.ConstraintEqualTo(BottomAnchor);
            NSLayoutConstraint.ActivateConstraints(new[] { right, left, top, bottom });
        }
    }
}