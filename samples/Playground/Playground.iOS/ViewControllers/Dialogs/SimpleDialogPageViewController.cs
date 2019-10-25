// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.iOS.Styles;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Dialogs
{
    public partial class SimpleDialogPageViewController : ViewControllerBase<SimpleDialogPageViewModel>
    {
        private readonly static Random _rand;

        static SimpleDialogPageViewController()
        {
            _rand = new Random();
        }

        public SimpleDialogPageViewController(IntPtr handle) : base(handle)
        {
            ModalPresentationStyle = (UIModalPresentationStyle) _rand.Next(1, 8);

            if(ModalPresentationStyle == UIModalPresentationStyle.Custom)
            {
                TransitioningDelegate = new DickViewControllerTransitioningDelegate();
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CloseButton.SetCommand(ViewModel.CloseCommand);
            DoneButton.SetCommand(ViewModel.DoneCommand);
        }
    }
}
