// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using Plugin.Connectivity.Abstractions;
using Softeq.XToolkit.Connectivity;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Threading;

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

        public string ConnectionTypes => string.Join(", ", _connectivityService.ConnectionTypes);

        public override void OnAppearing()
        {
            base.OnAppearing();

            _connectivityService.ConnectivityTypeChanged += (sender, args) =>
            {
                var isConnected = args.IsConnected;
                var connectionTypes = args.ConnectionTypes;

                if (isConnected && connectionTypes.Contains(ConnectionType.WiFi))
                {
                    // Wifi connected
                }
            };

            _connectivityService.ConnectivityChanged += ConnectivityServiceConnectivityChanged;
            _connectivityService.ConnectivityTypeChanged += ConnectivityServiceConnectivityTypeChanged;

            RaisePropertyChanged(nameof(ConnectionStatus));
            RaisePropertyChanged(nameof(ConnectionTypes));
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            _connectivityService.ConnectivityChanged -= ConnectivityServiceConnectivityChanged;
            _connectivityService.ConnectivityTypeChanged -= ConnectivityServiceConnectivityTypeChanged;
        }

        private void ConnectivityServiceConnectivityChanged(object sender, ConnectivityChangedEventArgs e)
        {
            Execute.BeginOnUIThread(() =>
            {
                RaisePropertyChanged(nameof(ConnectionStatus));
            });
        }

        private void ConnectivityServiceConnectivityTypeChanged(object sender, ConnectivityTypeChangedEventArgs e)
        {
            Execute.BeginOnUIThread(() =>
            {
                RaisePropertyChanged(nameof(ConnectionTypes));
            });
        }
    }
}
