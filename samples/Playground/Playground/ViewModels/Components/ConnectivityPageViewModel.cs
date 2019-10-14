using System;
using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Components
{
    public class ConnectivityPageViewModel : ViewModelBase
    {
        private readonly IConnectivityService _connectivityService;

        private string _connectionTypes;

        public ConnectivityPageViewModel(IConnectivityService connectivityService)
        {
            _connectivityService = connectivityService;

            _connectivityService.ConnectivityChanged += ConnectivityServiceConnectivityChanged;
            _connectivityService.ConnectivityTypeChanged += ConnectivityServiceConnectivityTypeChanged;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            RaisePropertyChanged(nameof(ConnectionStatus));
            RaisePropertyChanged(nameof(ConnectionTypes));
        }

        private void ConnectivityServiceConnectivityChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ConnectionStatus));
        }

        private void ConnectivityServiceConnectivityTypeChanged(object sender, Plugin.Connectivity.Abstractions.ConnectivityTypeChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(ConnectionTypes));
        }

        public string ConnectionStatus
        {
            get => _connectivityService.IsConnected ? "Connected" : "No Connection";
        }

        public string ConnectionTypes => string.Join(", ", _connectivityService.ConnectionTypes);
    }
}
