// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views
{
    [Activity(Label = "")]
    [SuppressMessage("ReSharper", "RedundantOverriddenMember", Justification = "Just for play.")]
    public class EmptyPageActivity : ActivityBase<EmptyPageViewModel>
    {
        private Button _openFlowButton;
        private Button _openEditProfileFlowButton;
        private Button _openTestApproachButton;
        private Button _openTestApproachWithFlowButton;
        private Button _openRibFlowButton;

        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_empty);

            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            _openFlowButton = FindViewById<Button>(Resource.Id.open_custom_flow_button);
            _openFlowButton.SetCommand(ViewModel.OpenCustomFlowCommand);

            _openEditProfileFlowButton = FindViewById<Button>(Resource.Id.open_edit_profile_flow_button);
            _openEditProfileFlowButton.SetCommand(ViewModel.OpenEditProfileNameFlowCommand);

            _openTestApproachButton = FindViewById<Button>(Resource.Id.open_test_approach_button);
            _openTestApproachButton.SetCommand(ViewModel.OpenTestApproachCommand);

            _openTestApproachWithFlowButton = FindViewById<Button>(Resource.Id.open_test_approach_with_flow_button);
            _openTestApproachWithFlowButton.SetCommand(ViewModel.OpenTestApproachWithFlowCommand);

            _openRibFlowButton = FindViewById<Button>(Resource.Id.open_rib_flow_button);
            _openRibFlowButton.SetCommand(ViewModel.OpenRibFlowCommand);
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            if (item.ItemId == Android.Resource.Id.Home)
            {
                OnBackPressed();
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        protected override void DoAttachBindings()
        {
            base.DoAttachBindings();

            // Put your code HERE.
        }

        protected override void DoDetachBindings()
        {
            base.DoDetachBindings();

            // Put your code HERE.
        }
    }
}
