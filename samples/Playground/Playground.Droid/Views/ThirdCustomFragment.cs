// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views
{
    public class ThirdCustomFragment : FragmentBase<ThirdCustomViewModel>
    {
        private Button? _closeCurrentFlowButton;
        private Button? _closeAllFlowsButton;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_custom_third, container, false);
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _closeCurrentFlowButton = view.FindViewById<Button>(Resource.Id.closee_current_flow_button);
            _closeCurrentFlowButton.SetCommand(ViewModel.CloseCurrentFlowCommand);

            _closeAllFlowsButton = view.FindViewById<Button>(Resource.Id.closee_all_flows_button);
            _closeAllFlowsButton.SetCommand(ViewModel.CloseAllFlowsCommand);
        }
    }
}
