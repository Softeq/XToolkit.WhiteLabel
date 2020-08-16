// Developed by Softeq Development Corporation
// http://www.softeq.com

using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public sealed class FrameNavigationConfig
    {
        public FrameNavigationConfig(FragmentManager fragmentManager, int containerId)
        {
            Manager = fragmentManager;
            ContainerId = containerId;
        }

        public FragmentManager Manager { get; }
        public int ContainerId { get; }
    }
}
