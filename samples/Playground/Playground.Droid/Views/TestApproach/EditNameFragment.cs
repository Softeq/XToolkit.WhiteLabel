// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.BLoC;
using Playground.ViewModels.CoordinatorPattern.ViewModels;
using Playground.ViewModels.TestApproach;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.TestApproach
{
    public class EditNameFragment : FragmentBase<BlocProvideNameViewModel>
    {
        private EditText? _profileNameEditText;
        private Button? _addButton;
        private object _closeButton;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.provide_profile_info_fragment, container, false);
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _profileNameEditText = view.FindViewById<EditText>(Resource.Id.profile_name_edittext);

            _addButton = view.FindViewById<Button>(Resource.Id.add_profile_button);
            _addButton.SetCommand(ViewModel.SaveCommand);

            _closeButton = view.FindViewById<Button>(Resource.Id.close_flow_button);
            _closeButton.SetCommand(ViewModel.CloseCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Name, () => _profileNameEditText.Text, BindingMode.TwoWay);
        }
    }
}
