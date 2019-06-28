// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using UIKit;

namespace Playground.iOS.Views
{
    public partial class MainPageItemViewCell : UITableViewCell
    {
        public static readonly NSString Key = new NSString("MainPageItemViewCell");
        public static readonly UINib Nib;

        static MainPageItemViewCell()
        {
            Nib = UINib.FromName("MainPageItemViewCell", NSBundle.MainBundle);
        }

        protected MainPageItemViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            SelectionStyle = UITableViewCellSelectionStyle.None;
        }

        internal void BindCell(CommandAction viewModel)
        {
            NameLabel.Text = viewModel.Title;
        }
    }
}