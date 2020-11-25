// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Playground.Forms.ViewModels.SimpleNavigation;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.TabbedNavigation
{
    public class Tab2PageViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public Tab2PageViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;

            ToNextPageCommand = new RelayCommand(ToNextPage);
        }

        public string Title => "Tab page 2";

        public ICommand ToNextPageCommand { get; }

        private void ToNextPage()
        {
            _frameNavigationService
              .For<SecondPageViewModel>()
              .From(this)
              .Navigate();
        }
    }
}
