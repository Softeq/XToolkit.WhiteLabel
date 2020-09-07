// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.App;
using Android.OS;
using Playground.ViewModels.Frames;
using Softeq.XToolkit.WhiteLabel.Droid;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;

namespace Playground.Droid.Views.Frames
{
    [Activity]
    public class FramesPageActivity : ActivityBase<FramesViewModel>
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.activity_frames);

            ViewModel.InitializeNavigation(
                new FrameNavigationConfig(
                    SupportFragmentManager,
                    Resource.Id.activity_frames_navigation));

            if (savedInstanceState == null)
            {
                ViewModel.RestoreState();
            }
        }

        public override void OnBackPressed()
        {
            if (ViewModel.TryHandleBackPress())
            {
                base.OnBackPressed();
            }
        }
    }
}
