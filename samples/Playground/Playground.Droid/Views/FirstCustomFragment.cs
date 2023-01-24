using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views
{
    public class FirstCustomFragment : FragmentBase<FirstCustomViewModel>
    {
        private Button? _openNewFlowButton;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_custom_first, container, false);
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _openNewFlowButton = view.FindViewById<Button>(Resource.Id.open_new_flow_button);
            _openNewFlowButton?.SetCommand(ViewModel.OpenNewCustomFlowCommand);
        }
    }
}
