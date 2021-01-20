// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class TabbedPageViewModel : ToolbarViewModelBase<string>
    {
        private readonly IPageNavigationService _pageNavigationService;

        public TabbedPageViewModel(IContainer container, IPageNavigationService pageNavigationService)
        {
            _pageNavigationService = pageNavigationService;

            TabModels = new List<TabItem<string>>
            {
                new TabItem<Tab1PageViewModel, string>("tab 1", "tab_first", container),
                new TabItem<Tab2PageViewModel, string>("tab 2", "tab_second", container)
            };

            OpenNewPageCommand = new RelayCommand(OpenNewPage);
        }

        public string Title => "Tabbed Page";

        public ICommand OpenNewPageCommand { get; }

        private void OpenNewPage()
        {
            _pageNavigationService
                .For<SettingsTabPageViewModel>()
                .Navigate();
        }
    }
}
