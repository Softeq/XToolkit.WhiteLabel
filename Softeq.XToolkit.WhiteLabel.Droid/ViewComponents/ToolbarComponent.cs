// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Android.OS;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public class ToolbarComponent<TViewModel, TKey>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        private readonly int _navigationContainerId;
        private int _oldSelectedIndex;

        public ToolbarComponent(int navigationContainerId)
        {
            _navigationContainerId = navigationContainerId;
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
                tabViewModel.InitializeNavigation(_navigationContainerId);
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
