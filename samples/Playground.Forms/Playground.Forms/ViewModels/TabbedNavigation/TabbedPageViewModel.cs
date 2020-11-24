// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class TabbedPageViewModel : ViewModelBase, ITabbedViewModel
    {
        private readonly IPageNavigationService _pageNavigationService;

        public TabbedPageViewModel(
            IPageNavigationService pageNavigationService,
            RootFrameNavigationPageViewModel<Tab1PageViewModel> tab1,
            RootFrameNavigationPageViewModel<Tab2PageViewModel> tab2)
        {
            _pageNavigationService = pageNavigationService;

            TabsViewModel = new List<IViewModelBase> { tab1, tab2 };
            OpenNewPageCommand = new RelayCommand(OpenNewPage);
        }

        public string? Title { get; set; } = "Tabbed Page";
        public IList<IViewModelBase> TabsViewModel { get; }
        public ICommand OpenNewPageCommand { get; }

        private void OpenNewPage()
        {
            _pageNavigationService
              .For<SettingsTabPageViewModel>()
              .Navigate();
        }
    }
}
