// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Dialogs
{
    public partial class DialogsPageViewController : ViewControllerBase<DialogsPageViewModel>
    {
        public DialogsPageViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ShowAlertButton.SetCommand(ViewModel.OpenAlertCommand);

            ShowDialogUntilDismissButton.SetCommand(ViewModel.OpenDialogUntilDismissCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.AlertResult, () => AlertResultLabel.Text);
            this.Bind(() => ViewModel.DialogUntilDismissResult, x =>
            {
                DialogUntilDismissResultLabel.Text = x?.FullName ?? "null";
            });
        }
    }
}
