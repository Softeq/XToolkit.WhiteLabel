// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Android.Widget;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Dialogs
{
    [Activity]
    public class DialogsPageActivity : ActivityBase<DialogsPageViewModel>
    {
        private Button? _openAlertButton;
        private TextView? _alertResultTextView;
        private Button? _openDialogUntilDismissButton;
        private TextView? _dialogDismissResultTextView;
        private Button? _openDialogUntilResultButton;
        private TextView? _dialogDismissResultResultTextView;
        private Button? _openTwoDialogsButton;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_dialogs);

            _openAlertButton = FindViewById<Button>(Resource.Id.activity_dialogs_alert_button);
            _alertResultTextView = FindViewById<TextView>(Resource.Id.activity_dialogs_alert_result_label);
            _openDialogUntilDismissButton = FindViewById<Button>(Resource.Id.activity_dialogs_dialog_dismiss_button);
            _dialogDismissResultTextView = FindViewById<TextView>(Resource.Id.activity_dialogs_dialog_dismiss_result_label);
            _openDialogUntilResultButton = FindViewById<Button>(Resource.Id.activity_dialogs_dialog_result_button);
            _dialogDismissResultResultTextView = FindViewById<TextView>(Resource.Id.activity_dialogs_dialog_result_label);
            _openTwoDialogsButton = FindViewById<Button>(Resource.Id.activity_dialogs_two_dialogs_button);

            _openAlertButton.SetCommand(ViewModel.OpenAlertCommand);
            _openDialogUntilDismissButton.SetCommand(ViewModel.OpenDialogUntilDismissCommand);
            _openDialogUntilResultButton.SetCommand(ViewModel.OpenDialogUntilResultCommand);
            _openTwoDialogsButton.SetCommand(ViewModel.OpenTwoDialogsCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.AlertResult, () => _alertResultTextView!.Text);
            this.Bind(() => ViewModel.DialogUntilDismissResult, () => _dialogDismissResultTextView!.Text, ViewModel.PersonConverter);
            this.Bind(() => ViewModel.DialogUntilResult, () => _dialogDismissResultResultTextView!.Text, ViewModel.PersonConverter);
        }
    }
}
