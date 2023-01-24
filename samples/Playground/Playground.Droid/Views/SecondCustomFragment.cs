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
    public class SecondCustomFragment : FragmentBase<SecondCustomViewModel>
    {
        private Button? _navigateToNextScreenButton;

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_custom_second, container, false);
        }

        public override void OnViewCreated(View view, Bundle? savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            _navigateToNextScreenButton = view.FindViewById<Button>(Resource.Id.navigate_to_third_custom_screen_button);
            _navigateToNextScreenButton.SetCommand(ViewModel.OpenNextScreenCommand);
        }
    }
}
