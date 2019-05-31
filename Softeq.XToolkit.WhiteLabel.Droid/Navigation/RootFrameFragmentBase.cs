// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Views;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class RootFrameFragmentBase<T> : FragmentBase<T> where T : RootFrameNavigationViewModelBase
    {
        private FrameNavigationService _frameNavigationService;

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var frame = inflater.Inflate(Resource.Layout.fragment_navigation_root, container, false);

            var view = frame.FindViewById<View>(Resource.Id.fragment_navigation_root_frame_layout);
            var id = View.GenerateViewId();
            view.Id = id;
            _frameNavigationService = ((FrameNavigationService) ViewModel.FrameNavigationService);

            _frameNavigationService.Initialize(id);
            ViewModel.RestoreState();

            return frame;
        }

        public override void OnResume()
        {
            base.OnResume();

            var fragment = _frameNavigationService.GetTopFragment();

            //if fragment already on screen otherwise fragment will cause itself 
            if (fragment.IsVisible)
            {
                fragment.OnResume();
            }
        }

        public override void OnPause()
        {
            base.OnPause();

            var fragment = _frameNavigationService.GetTopFragment();

            //if fragment already on screen otherwise fragment will cause itself 
            if (fragment.IsVisible)
            {
                fragment.OnPause();
            }
        }
    }
}