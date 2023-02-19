// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Softeq.XToolkit.Bindings.iOS.Bindable;
using UIKit;

namespace Playground.iOS.Views
{
    public partial class MainPageGroupHeaderViewCell : BindableTableViewHeaderFooterView<string>
    {
        public static readonly NSString Key = new NSString(nameof(MainPageGroupHeaderViewCell));
        public static readonly UINib Nib;

        static MainPageGroupHeaderViewCell() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected MainPageGroupHeaderViewCell(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

            NameLabel.Text = ViewModel;
        }
    }
}
