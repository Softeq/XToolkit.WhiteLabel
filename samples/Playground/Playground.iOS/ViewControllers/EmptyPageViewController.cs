// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers
{
    public partial class EmptyPageViewController : ViewControllerBase<EmptyPageViewModel>
    {
        public EmptyPageViewController(IntPtr handle) : base(handle)
        {
        }

        ~EmptyPageViewController()
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();
        }

        protected override void DoDetachBindings()
        {
            base.DoDetachBindings();
        }
    }
}
