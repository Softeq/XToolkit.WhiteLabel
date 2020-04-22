# XToolkit Connectivity

Library over the [Xam.Plugin.Connectivity](https://github.com/jamesmontemagno/ConnectivityPlugin) with support iOS 12+ native API.

## Install

Currently can be used only as source code (via git submodules).

## Description

Please, use [IConnectivityService](xref:Softeq.XToolkit.Connectivity.IConnectivityService) to check and observe connection status. You can find some usage examples below:

### Check connection

```cs
var isConnected = _connectivityService.IsConnected;
```

### Connection changed

```cs
_connectivityService.ConnectivityChanged += (sender, args) =>
{
    var isConnected = args.IsConnected;
};
```

### Connection type changed

```cs
_connectivityService.ConnectivityTypeChanged += (sender, args) =>
{
    var isConnected = args.IsConnected;
    var connectionTypes = args.ConnectionTypes;

    if (isConnected && connectionTypes.Contains(ConnectionType.WiFi))
    {
        // WiFi connected
    }
};
```

---
