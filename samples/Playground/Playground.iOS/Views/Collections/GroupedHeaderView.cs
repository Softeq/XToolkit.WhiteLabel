// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Playground.iOS.ViewControllers.Collections;
using UIKit;

namespace Playground.iOS.Views.Collections
{
    public partial class GroupedHeaderView : BindableUICollectionReusableView<string>
    {
        #region init

        public static readonly NSString Key = new NSString(nameof(GroupedHeaderView));
        public static readonly UINib Nib;

        static GroupedHeaderView() => Nib = UINib.FromName(Key, NSBundle.MainBundle);

        protected GroupedHeaderView(IntPtr handle) : base(handle)
        {
            // Note: this .ctor should not contain any initialization logic.
        }

        #endregion

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

        }
    }
}
