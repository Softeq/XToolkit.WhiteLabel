// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.Graphics;
using Android.OS;
using Android.Views;
using Android.Widget;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.Bindings;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Droid;

namespace Playground.Droid.Views.Frames
{
    public class YellowFragment : FragmentBase<YellowViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_with_navigation, container, false);
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            var next = View.FindViewById<Button>(Resource.Id.fragment_with_navigation_next);
            next.Visibility = ViewStates.Gone;

            var back = View.FindViewById<Button>(Resource.Id.fragment_with_navigation_back);
            back.SetCommand(new RelayCommand(ViewModel.GoBack));

            var container = View.FindViewById<View>(Resource.Id.fragment_with_navigation_view);
            container.SetBackgroundColor(Color.Yellow);
        }
    }
}
