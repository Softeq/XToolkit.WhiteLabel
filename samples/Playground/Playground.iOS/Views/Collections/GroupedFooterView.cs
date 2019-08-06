// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Playground.iOS.ViewControllers.Collections;
using UIKit;

namespace Playground.iOS.Views.Collections
{
    [Register(nameof(GroupedFooterView))]
    public class GroupedFooterView : BindableUICollectionReusableView<string>
    {
        public GroupedFooterView(IntPtr handle) : base(handle)
        {

            BackgroundColor = UIColor.Blue;
        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

        }
    }
}
