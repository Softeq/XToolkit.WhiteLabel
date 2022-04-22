// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Forms.Navigation;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.Forms.ViewModels.MasterDetailNavigation.DrillNavigation
{
    public class DrillLevel2PageViewModel : ViewModelBase
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        private readonly IFrameNavigationService _frameNavigationService;

        public DrillLevel2PageViewModel(IFrameNavigationService frameNavigationService)
        {
            _frameNavigationService = frameNavigationService;

            GoBackCommand = new RelayCommand(() =>
            {
                _frameNavigationService
                    .From(this)
                    .For<DrillLevel2PageViewModel>()
                    .NavigateBack();
            });
        }

        public ICommand GoBackCommand { get; }
    }
}
