// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Android.OS;
using Android.Views;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class RootFrameFragmentBase<T> : FragmentBase<T>
        where T : RootFrameNavigationViewModelBase
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var frame = inflater.Inflate(Resource.Layout.fragment_navigation_root, container, false);

            var view = frame.FindViewById<View>(Resource.Id.fragment_navigation_root_frame_layout);
            var id = View.GenerateViewId();
            view.Id = id;

            ViewModel.FrameNavigationService.Initialize(id);
            ViewModel.RestoreState();

            return frame;
        }
    }
}
