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
        private Button _openAlertButton;
        private TextView _alertResultTextView;
        private Button _openDialogUntilDismiss;
        private TextView _dialogDismissResultTextView;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_dialogs);

            _openAlertButton = FindViewById<Button>(Resource.Id.activity_dialogs_alert_button);
            _alertResultTextView = FindViewById<TextView>(Resource.Id.activity_dialogs_alert_result_label);
            _openDialogUntilDismiss = FindViewById<Button>(Resource.Id.activity_dialogs_dialog_dismiss_button);
            _dialogDismissResultTextView = FindViewById<TextView>(Resource.Id.activity_dialogs_dialog_dismiss_result_label);

            _openAlertButton.SetCommand(ViewModel.OpenAlertCommand);
            _openDialogUntilDismiss.SetCommand(ViewModel.OpenDialogUntilDismissCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.AlertResult, () => _alertResultTextView.Text);
            this.Bind(() => ViewModel.DialogUntilDismissResult, x =>
            {
                _dialogDismissResultTextView.Text = x?.FullName ?? "null";
            });
        }
    }
}
