// Developed by Softeq Development Corporation
// http://www.softeq.com

using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation.FrameNavigation
{
    public sealed class FrameNavigationConfig
    {
        private readonly int _containerId;

        private FragmentState? _currentFragment;

        public FrameNavigationConfig(FragmentManager fragmentManager, int containerId)
        {
            Manager = fragmentManager;
            _containerId = containerId;
        }

        public FragmentManager Manager { get; }

        public bool CanReplaceFragment => !Manager.IsDestroyed && !Manager.IsStateSaved;

        public FragmentTransaction ReplaceFragment(FragmentState fragment)
        {
            _currentFragment?.SaveFragmentState(Manager);
            _currentFragment = fragment;

            var preparedFragment = _currentFragment.PrepareFragment();

            return Manager
                .BeginTransaction()
                .Replace(_containerId, preparedFragment);
        }
    }
}
