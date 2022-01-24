// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Windows.Input;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.ViewModels.Components
{
    public class LauncherPageViewModel : ViewModelBase
    {
        private readonly ILauncherService _launcherService;

        public LauncherPageViewModel(ILauncherService launcherService)
        {
            _launcherService = launcherService;

            OpenUrlCommand = new RelayCommand(() => _launcherService.OpenUrl("https://softeq.com"));
            OpenAppSettingsCommand = new RelayCommand(() => _launcherService.OpenAppSettings());
            OpenVideoCommand = new RelayCommand(() => _launcherService.OpenVideo("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
            OpenEmailCommand = new RelayCommand(() => _launcherService.OpenEmail("test@example.com"));
            OpenPhoneNumberCommand = new RelayCommand(() => _launcherService.OpenPhoneNumber("1234455"));
        }

        public ICommand OpenUrlCommand { get; }

        public ICommand OpenAppSettingsCommand { get; }

        public ICommand OpenVideoCommand { get; }

        public ICommand OpenEmailCommand { get; }

        public ICommand OpenPhoneNumberCommand { get; }
    }
}
