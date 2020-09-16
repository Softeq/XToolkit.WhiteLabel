// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Frames
{
    public class RedFragment : FragmentBase<RedViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_with_navigation, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var next = View.FindViewById<Button>(Resource.Id.fragment_with_navigation_next);
            next.SetCommand(ViewModel.NextCommand);
            next.Text = ViewModel.NextText;

            var back = View.FindViewById<Button>(Resource.Id.fragment_with_navigation_back);
            back.SetCommand(ViewModel.BackCommand);
            back.Text = ViewModel.BackText;

            var container = View.FindViewById<View>(Resource.Id.fragment_with_navigation_view);
            container.SetBackgroundColor(Color.Red);
        }
    }
}
