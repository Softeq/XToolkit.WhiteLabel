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
    public class Tab2PageViewModel : ViewModelBase, ITabViewModel
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public Tab2PageViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;

            ToNextPageCommand = new RelayCommand(ToNextPage);
        }

        public string? Title => "Page 2";
        public string? IconImageSource => "AppIcon";

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
