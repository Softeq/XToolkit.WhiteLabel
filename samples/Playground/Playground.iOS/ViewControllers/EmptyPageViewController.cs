// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics.CodeAnalysis;
using Playground.ViewModels;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers
{
    [SuppressMessage("ReSharper", "EmptyDestructor", Justification = "Just for play.")]
    [SuppressMessage("ReSharper", "RedundantOverriddenMember", Justification = "Just for play.")]
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

            // Put your code HERE.
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            // Put your code HERE.
        }

        protected override void DoDetachBindings()
        {
            base.DoDetachBindings();

            // Put your code HERE.
        }
    }
}
