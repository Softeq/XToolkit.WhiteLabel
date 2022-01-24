// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
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

            OpenUrlCommand = new RelayCommand(OpenUrl);
            OpenAppSettingsCommand = new RelayCommand(OpenAppSettings);
            OpenVideoCommand = new RelayCommand(OpenVideo);
            OpenEmailCommand = new RelayCommand(OpenEmail);
            OpenPhoneNumberCommand = new RelayCommand(OpenPhoneNumber);
        }

        public ICommand OpenUrlCommand { get; }

        public ICommand OpenAppSettingsCommand { get; }

        public ICommand OpenVideoCommand { get; }

        public ICommand OpenEmailCommand { get; }

        public ICommand OpenPhoneNumberCommand { get; }

        private void OpenUrl()
        {
            Task.Run(() => _launcherService.OpenUrl("https://softeq.com"));
        }

        private void OpenAppSettings()
        {
            Task.Run(() => _launcherService.OpenAppSettings());
        }

        private void OpenVideo()
        {
            Task.Run(() => _launcherService.OpenVideo("https://commondatastorage.googleapis.com/gtv-videos-bucket/sample/BigBuckBunny.mp4"));
        }

        private void OpenEmail()
        {
            Task.Run(() => _launcherService.OpenEmail("test@example.com"));
        }

        private void OpenPhoneNumber()
        {
            Task.Run(() => _launcherService.OpenPhoneNumber("1234455"));
        }
    }
}
