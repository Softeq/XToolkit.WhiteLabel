// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.Dialogs;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid.Dialogs;

namespace Playground.Droid.Views.Dialogs
{
    public class SimpleDialogPageFragment : DialogFragmentBase<SimpleDialogPageViewModel>
    {
        private Button? _closeButton;
        private Button? _doneButton;

        // TODO YP: move inflate to WL
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return LayoutInflater.Inflate(Resource.Layout.dialog_simple_dialog, container, true);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _closeButton = view.FindViewById<Button>(Resource.Id.dialog_simple_dialog_close_button);
            _doneButton = view.FindViewById<Button>(Resource.Id.dialog_simple_dialog_done_button);

            _closeButton.SetCommand(ViewModel.CloseCommand);
            _doneButton.SetCommand(ViewModel.DoneCommand);
        }
    }
}
