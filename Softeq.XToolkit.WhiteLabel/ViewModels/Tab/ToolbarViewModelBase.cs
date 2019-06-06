// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Autofac;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.Tab;

namespace Softeq.XToolkit.WhiteLabel.ViewModels.Tab
{
    public abstract class ToolbarViewModelBase : ViewModelBase
    {
        private readonly ITabNavigationService _tabNavigationService;

        public ToolbarViewModelBase(ITabNavigationService tabNavigationService)
        {
            _tabNavigationService = tabNavigationService;

            SelectionChangedCommand = new RelayCommand<int>(SelectionChanged);

            GoBackCommand = new RelayCommand(GoBack);
        }

        public IList<TabItem> TabModels { get; set; }

        public bool CanGoBack => _tabNavigationService.CanGoBack;

        public IList<RootFrameNavigationViewModel> TabViewModels { get; private set; }

        public ICommand GoBackCommand { get; }

        public ICommand SelectionChangedCommand { get; }

        public int SelectedIndex { get; private set; }

        public override void OnInitialize()
        {
            base.OnInitialize();
            TabViewModels = new List<RootFrameNavigationViewModel>(TabModels
                .Select(x => Dependencies.IocContainer.Resolve<RootFrameNavigationViewModel>(new NamedParameter("model", x))));
            _tabNavigationService.SetSelectedViewModel(TabViewModels[SelectedIndex]);
        }

        private void SelectionChanged(int selectedViewModel)
        {
            if (Equals(SelectedIndex, selectedViewModel))
            {
                return;
            }

            SelectedIndex = selectedViewModel;
            _tabNavigationService.SetSelectedViewModel(TabViewModels[SelectedIndex]);
        }

        private void GoBack()
        {
            _tabNavigationService.GoBack();
        }
    }
}