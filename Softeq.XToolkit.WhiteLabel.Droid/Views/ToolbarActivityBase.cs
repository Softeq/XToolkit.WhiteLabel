// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Android.OS;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.Views
{
    public abstract class ToolbarActivityBase<TViewModel, TKey> : ActivityBase<TViewModel>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        private int _oldSelectedIndex;
        protected abstract int NavigationContainer { get; }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            var wasInit = ViewModel.IsInitialized;

            base.OnCreate(savedInstanceState);

            if (wasInit) // HACK YP: need another way
            {
                return;
            }

            foreach (var tabViewModel in ViewModel.TabViewModels)
            {
                tabViewModel.InitializeNavigation(NavigationContainer);
            }

            var selectedTabViewModel = ViewModel.TabViewModels.FirstOrDefault();
            selectedTabViewModel?.NavigateToFirstPage();
        }

        protected void TabSelected(int newSelectedIndex)
        {
            var selectedTabViewModel = ViewModel.TabViewModels.ElementAt(newSelectedIndex);

            if (newSelectedIndex == _oldSelectedIndex) // fast-backward nav
            {
                selectedTabViewModel.NavigateToFirstPage();
            }
            else
            {
                selectedTabViewModel.RestoreState();
            }

            _oldSelectedIndex = newSelectedIndex;
        }

        public override void OnBackPressed()
        {
            var viewModel = ViewModel.TabViewModels[_oldSelectedIndex];
            if (viewModel.CanGoBack)
            {
                viewModel.GoBack();
            }
            else
            {
                base.OnBackPressed();
            }
        }
    }
}