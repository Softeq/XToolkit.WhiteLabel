// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using ObjCRuntime;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.iOS;
using UIKit;

namespace Playground.iOS.ViewControllers.Dialogs
{
    public sealed partial class SimpleDialogPageViewController : ViewControllerBase<SimpleDialogPageViewModel>
    {
        private static readonly Random _rand = new Random();

        public SimpleDialogPageViewController(NativeHandle handle) : base(handle)
        {
            ModalPresentationStyle = (UIModalPresentationStyle) _rand.Next(1, 8);

            if (ModalPresentationStyle == UIModalPresentationStyle.Custom)
            {
                TransitioningDelegate = new CustomTransitioningDelegate();
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
