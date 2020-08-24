// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Softeq.XToolkit.WhiteLabel.Droid.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Softeq.XToolkit.WhiteLabel.Droid.ViewComponents
{
    public class ToolbarComponent<TViewModel, TKey>
        where TViewModel : ToolbarViewModelBase<TKey>
    {
        private readonly FrameNavigationConfig _frameNavigationConfig;

        public ToolbarComponent(FrameNavigationConfig frameNavigationConfig)
        {
            _frameNavigationConfig = frameNavigationConfig;
        }

        public void Initialize(TViewModel viewModel)
        {
            foreach (var tabViewModel in viewModel.TabViewModels)
            {
                tabViewModel.InitializeNavigation(_frameNavigationConfig);
            }

            var selectedTabViewModel = viewModel.TabViewModels[viewModel.SelectedIndex];
            selectedTabViewModel?.RestoreState();
        }

        public void TabSelected(TViewModel viewModel, int newSelectedIndex)
        {
            var selectedTabViewModel = viewModel.TabViewModels.ElementAt(newSelectedIndex);

            if (newSelectedIndex == viewModel.SelectedIndex) // fast-backward nav
            {
                selectedTabViewModel.NavigateToFirstPage();
            }
            else
            {
                selectedTabViewModel.RestoreState();
            }

            viewModel.SelectedIndex = newSelectedIndex;
        }

        public bool HandleBackPress(TViewModel viewModel)
        {
            var vm = viewModel.TabViewModels[viewModel.SelectedIndex];
            if (vm.CanGoBack)
            {
                vm.GoBack();
                return true;
            }

            return false;
        }
    }
}
