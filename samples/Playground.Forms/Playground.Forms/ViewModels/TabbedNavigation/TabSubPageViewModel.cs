// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class TabSubPageViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public TabSubPageViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;

            BackCommand = new RelayCommand(Back);
        }

        public string Title => "Sub page";

        public ICommand BackCommand { get; }

        private void Back()
        {
            _frameNavigationService
                .For<TabSubPageViewModel>()
                .From(this)
                .NavigateBack();
        }
    }
}
