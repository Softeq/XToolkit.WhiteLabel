// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Foundation;
using Playground.iOS.ViewControllers.Collections;

namespace Playground.iOS.Views.Collections
{
    [Register(nameof(GroupedFooterView))]
    public class GroupedFooterView : BindableUICollectionReusableView<string>
    {
        public GroupedFooterView(IntPtr handle) : base(handle)
        {
        }

        public override void AwakeFromNib()
        {
            base.AwakeFromNib();


        }

        public override void DoAttachBindings()
        {
            base.DoAttachBindings();

        }
    }
}
