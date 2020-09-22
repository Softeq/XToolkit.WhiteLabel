// Developed by Softeq Development Corporation
// http://www.softeq.com

using Android.OS;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.Droid.ViewComponents;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class ToolbarFragmentBase<TViewModel, TKey> : FragmentBase<TViewModel>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        private BackPressedCallback? _backPressedCallback;

        protected abstract ToolbarComponent<TViewModel, TKey> ToolbarComponent { get; }

        public override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            ToolbarComponent.Initialize(ViewModel);

            _backPressedCallback = new BackPressedCallback(true, HandleBackPressed);
            Activity.OnBackPressedDispatcher.AddCallback(this, _backPressedCallback);
        }

        public override void OnDestroy()
        {
            base.OnDestroy();

            _backPressedCallback?.Detach();
            _backPressedCallback = null;
        }

        private void HandleBackPressed()
        {
            if (!ToolbarComponent.HandleBackPress(ViewModel))
            {
                if (_backPressedCallback != null)
                {
                    _backPressedCallback.Enabled = false;
                }

                Activity.OnBackPressed();

                if (_backPressedCallback != null)
                {
                    _backPressedCallback.Enabled = true;
                }
            }
        }
    }
}
