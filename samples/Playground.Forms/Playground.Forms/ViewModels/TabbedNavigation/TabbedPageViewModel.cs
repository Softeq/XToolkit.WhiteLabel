// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Model;
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

            TabbedItems = new List<TabbedItem>
            {
                new TabbedItem("tab 1", tab1, "tab_first"),
                new TabbedItem("tab 2", tab2, "tab_second"),
            };

            OpenNewPageCommand = new RelayCommand(OpenNewPage);
        }

        public string Title => "Tabbed Page";

        public IList<TabbedItem> TabbedItems { get; }
        public ICommand OpenNewPageCommand { get; }

        private void OpenNewPage()
        {
            _pageNavigationService
                .For<SettingsTabPageViewModel>()
                .Navigate();
        }
    }
}
