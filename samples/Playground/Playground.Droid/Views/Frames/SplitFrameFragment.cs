// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Android.Views;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;

namespace Playground.Droid.Views.Frames
{
    public class SplitFrameFragment : FragmentBase<SplitFrameViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            return inflater.Inflate(Resource.Layout.fragment_split, container, false);
        }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ViewModel.TopShellViewModel.InitializeNavigation(
                new FrameNavigationConfig(
                    ChildFragmentManager,
                    Resource.Id.fragment_split_frame1));

            if (savedInstanceState == null)
            {
                ViewModel.TopShellViewModel.RestoreState();
            }
        }
    }
}
