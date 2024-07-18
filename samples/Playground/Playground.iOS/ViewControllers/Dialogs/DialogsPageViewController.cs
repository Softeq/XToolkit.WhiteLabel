// Developed by Softeq Development Corporation
// http://www.softeq.com

using ObjCRuntime;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.iOS;

namespace Playground.iOS.ViewControllers.Dialogs
{
    public partial class DialogsPageViewController : ViewControllerBase<DialogsPageViewModel>
    {
        public DialogsPageViewController(NativeHandle handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            ShowAlertButton.SetCommand(ViewModel.OpenAlertCommand);
            ShowConfirmButton.SetCommand(ViewModel.OpenConfirmCommand);
            ShowActionSheetButton.SetCommand(ViewModel.OpenActionSheetCommand);
            ShowDialogUntilDismissButton.SetCommand(ViewModel.OpenDialogUntilDismissCommand);
            ShowDialogUntilResultButton.SetCommand(ViewModel.OpenDialogUntilResultCommand);
            OpenTwoDialogsButton.SetCommand(ViewModel.OpenTwoDialogsCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.AlertResult, () => AlertResultLabel.Text);
            this.Bind(() => ViewModel.ConfirmResult, () => ConfirmResultLabel.Text);
            this.Bind(() => ViewModel.ActionSheetResult, () => ActionSheetResultLabel.Text);
            this.Bind(() => ViewModel.DialogUntilDismissResult, () => DialogUntilDismissResultLabel.Text, ViewModel.PersonConverter);
            this.Bind(() => ViewModel.DialogUntilResult, () => ShowDialogUntilResultLabel.Text, ViewModel.PersonConverter);
        }
    }
}
