// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using UIKit;

namespace Playground.iOS.Views.Collections
{
    public partial class DummyCell : UICollectionViewCell
    {
        public static readonly NSString Key = new NSString(nameof(DummyCell));
        public static readonly UINib Nib;

        static DummyCell() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected DummyCell(IntPtr handle)
            : base(handle)
        {
        }

        public void Configure(string text)
        {
            TitleLabel.Text = text;
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            Layer.BorderColor = UIColor.Black.CGColor;
            Layer.BorderWidth = 1.0f;
        }
    }
}
