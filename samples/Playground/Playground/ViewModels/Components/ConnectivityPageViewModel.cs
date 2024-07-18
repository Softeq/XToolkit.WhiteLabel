// Developed by Softeq Development Corporation
// http://www.softeq.com

using Microsoft.Maui.Networking;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Components
{
    public class ConnectivityPageViewModel : ViewModelBase
    {
        private readonly IConnectivityService _connectivityService;

        public ConnectivityPageViewModel(IConnectivityService connectivityService)
        {
            _connectivityService = connectivityService;
        }

        public string ConnectionStatus => _connectivityService.IsConnected ? "Connected" : "No Connection";

        public string ConnectionProfiles => string.Join(", ", _connectivityService.ConnectionProfiles);

        public override void OnAppearing()
        {
            base.OnAppearing();

            _connectivityService.ConnectivityChanged += ConnectivityServiceConnectivityChanged;

            UpdateStates();
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            _connectivityService.ConnectivityChanged -= ConnectivityServiceConnectivityChanged;
        }

        private void ConnectivityServiceConnectivityChanged(object? sender, ConnectivityChangedEventArgs e)
        {
            UpdateStates();
        }

        private void UpdateStates()
        {
            Execute.BeginOnUIThread(() =>
            {
                RaisePropertyChanged(nameof(ConnectionStatus));
                RaisePropertyChanged(nameof(ConnectionProfiles));
            });
        }
    }
}
