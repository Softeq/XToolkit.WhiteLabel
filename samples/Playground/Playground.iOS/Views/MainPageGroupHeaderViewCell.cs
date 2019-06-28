// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using UIKit;

namespace Playground.iOS.Views
{
    public partial class MainPageGroupHeaderViewCell : UITableViewHeaderFooterView
    {
        public static readonly NSString Key = new NSString("MainPageGroupHeaderViewCell");
        public static readonly UINib Nib;

        static MainPageGroupHeaderViewCell()
        {
            Nib = UINib.FromName("MainPageGroupHeaderViewCell", NSBundle.MainBundle);
        }

        protected MainPageGroupHeaderViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        internal void BindCell(string headerKey)
        {
            NameLabel.Text = headerKey;
        }
    }
}