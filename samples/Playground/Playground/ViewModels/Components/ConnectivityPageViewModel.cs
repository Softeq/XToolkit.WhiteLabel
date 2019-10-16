// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using Plugin.Connectivity.Abstractions;
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

            _connectivityService.ConnectivityChanged += ConnectivityServiceConnectivityChanged;
            _connectivityService.ConnectivityTypeChanged += ConnectivityServiceConnectivityTypeChanged;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            RaisePropertyChanged(nameof(ConnectionStatus));
            RaisePropertyChanged(nameof(ConnectionTypes));
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

        public string ConnectionStatus
        {
            get => _connectivityService.IsConnected ? "Connected" : "No Connection";
        }

        public string ConnectionTypes => string.Join(", ", _connectivityService.ConnectionTypes);
    }
}
