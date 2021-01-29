// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.MasterDetailNavigation.DrillNavigation
{
    public class DrillLevel1PageViewModel : ViewModelBase
    {
        private readonly IFrameNavigationService _frameNavigationService;

        public DrillLevel1PageViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;

            NavigateCommand = new RelayCommand(() =>
            {
                _frameNavigationService
                    .For<DrillLevel2PageViewModel>()
                    .From(this)
                    .Navigate();
            });
        }

        public ICommand NavigateCommand { get; }
    }
}
