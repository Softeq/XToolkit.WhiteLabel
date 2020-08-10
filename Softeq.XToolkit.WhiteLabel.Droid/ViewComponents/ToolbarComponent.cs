// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Android.OS;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public class ToolbarComponent<TViewModel, TKey>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        private readonly FrameNavigationConfig _frameNavigationConfig;

        private int _oldSelectedIndex;

        public ToolbarComponent(FrameNavigationConfig frameNavigationConfig)
        {
            _frameNavigationConfig = frameNavigationConfig;
        }

        public void OnCreate(TViewModel viewModel, Action<Bundle> onCreate, Bundle savedInstanceState)
        {
            var wasInit = viewModel.IsInitialized;

            onCreate(savedInstanceState);

            if (wasInit) // HACK YP: need another way
            {
                return;
            }

            foreach (var tabViewModel in viewModel.TabViewModels)
            {
                tabViewModel.InitializeNavigation(_frameNavigationConfig);
            }

            var selectedTabViewModel = viewModel.TabViewModels.FirstOrDefault();
            selectedTabViewModel?.NavigateToFirstPage();
        }

        public void TabSelected(TViewModel viewModel, int newSelectedIndex)
        {
            var selectedTabViewModel = viewModel.TabViewModels.ElementAt(newSelectedIndex);

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

        public bool HandleBackPress(TViewModel viewModel)
        {
            var vm = viewModel.TabViewModels[_oldSelectedIndex];
            if (vm.CanGoBack)
            {
                vm.GoBack();
                return true;
            }

            return false;
        }
    }
}
