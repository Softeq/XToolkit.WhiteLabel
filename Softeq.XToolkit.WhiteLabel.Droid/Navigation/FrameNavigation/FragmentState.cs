// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using AndroidX.Fragment.App;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation.FrameNavigation
{
    public sealed class FragmentState
    {
        private readonly Fragment _fragment;
        private readonly Type _viewModelType;

        private Fragment.SavedState? _savedState;

        public FragmentState(Fragment fragment, Type viewModelType)
        {
            _fragment = fragment;
            _viewModelType = viewModelType;

            Key = fragment.GetType().Name;
        }

        public string Key { get; }

        public bool IsViewModelOfType<T>()
        {
            return typeof(T) == _viewModelType;
        }

        public Fragment PrepareFragment()
        {
            if (!_fragment.IsAdded && _savedState != null)
            {
                _fragment.SetInitialSavedState(_savedState);
            }

            return _fragment;
        }

        public void SaveFragmentState(FragmentManager fragmentManager)
        {
            if (_fragment.IsAdded)
            {
                _savedState = fragmentManager.SaveFragmentInstanceState(_fragment);
            }
        }
    }
}
