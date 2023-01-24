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
    public class DetailsFragement : FragmentBase<BlocDetailViewModel>
    {
        private TextView? _profileTextView;
        private Button? _editButton;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.profile_details_fragment, container, false);
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _profileTextView = view.FindViewById<TextView>(Resource.Id.profile_name_textview);

            _editButton = view.FindViewById<Button>(Resource.Id.edit_profile_button);
            _editButton.SetCommand(ViewModel.EditCommand);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            this.Bind(() => ViewModel.Name, () => _profileTextView.Text, BindingMode.TwoWay);
        }
    }
}
