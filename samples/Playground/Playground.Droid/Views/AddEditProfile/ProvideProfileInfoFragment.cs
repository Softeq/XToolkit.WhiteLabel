// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.AddEditProfile;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Bindings.Extensions;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.AddEditProfile
{
    public class ProvideProfileInfoFragment : FragmentBase<ProvideProfileInfoViewModel>
    {
        private EditText? _profileNameEditText;
        private Button? _addButton;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.provide_profile_info_fragment, container, false);
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _profileNameEditText = view.FindViewById<EditText>(Resource.Id.profile_name_edittext);

            _addButton = view.FindViewById<Button>(Resource.Id.add_profile_button);
            _addButton.SetCommand(ViewModel.AddCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Name, () => _profileNameEditText.Text);
        }
    }
}
