# XToolkit Remote

## Overview

An opinionated HTTP library for Mobile Development. It provides a common way to make HTTP requests. It helps you to write more efficient code in mobile and desktop applications written in C#.

### Features

- HttpClient builder
- Cancellation
- Throwing exceptions (system exceptions preferred)
- Logger & diagnostic
- Retry
- Refit
- Auth with refresh token

### Layers

- **HttpClientBuilder** - create & configure HttpClient;
- **ApiService** - create API service implementation based on Refit or custom implementation;
- **RemoteService** - make service calls with retry & catch exceptions;

## Install

When you use this component separately from WhiteLabel.

You can install via NuGet: [![Softeq.XToolkit.Remote](https://buildstats.info/nuget/Softeq.XToolkit.Remote?includePreReleases=true)](https://www.nuget.org/packages/Softeq.XToolkit.Remote)

    Install-Package Softeq.XToolkit.Remote

## Usage

These are the steps to describe a simple way to make an HTTP request:

### Step 1

Firstly you need to create an interface for declaring API endpoints:

```cs
public interface IApi
{
    [Get("/profile")]
    Task<string> GetProfile(CancellationToken ct);
}
```

> [Refit](https://github.com/reactiveui/refit#api-attributes) library is used for declaring the API endpoints.

### Step 2

Create simple HttpClient:

```cs
var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://example.com/api")
};
```

> The library provides an advanced way to create HttpClient. See [DefaultHttpClientFactory](xref:Softeq.XToolkit.Remote.Client.DefaultHttpClientFactory), [HttpClientBuilder](xref:Softeq.XToolkit.Remote.Client.HttpClientBuilder).

### Step 3

Create instance of RemoteService:

```cs
var remoteServiceFactory = new RemoteServiceFactory();

var remoteService = remoteServiceFactory.Create<IApi>(httpClient);
```

> In this case, it can be used in a **simpler way**:
```cs
var remoteService = remoteServiceFactory.Create<IApi>("https://example.com/api");
```

### Step 4

Make simple request:

```cs
var result = await remoteService.MakeRequest(
    (service, cancellationToken) =>
        service.GetProfile(cancellationToken));
```

or safe call example:

```cs
ILogger logger = ...;

var result = await remoteService.SafeRequest(
    (service, cancellationToken) =>
        service.GetProfile(cancellationToken),
    CancellationToken.None, // optional: parent token
    logger)
````

## Advanced

### Custom primary handler

Declare custom primary handler:

```cs
var customPrimaryHandler = new SocketsHttpHandler();
```

Use custom handler for HttpClientBuilder:

```cs
var messageHandlerBuilder = new DefaultHttpMessageHandlerBuilder(customPrimaryHandler);

var httpClientBuilder = new HttpClientBuilder(messageHandlerBuilder);

var httpClient = httpClientBuilder
    .WithBaseUrl("https://softeq.com")
    .Build();
```

### Setup HttpClient

> HttpClient <- [DefaultHttpClientFactory] <- HttpClientBuilder <- [DefaultHttpMessageHandlerBuilder] <- HttpMessageHandler

#### Enable logging

Create HttpClient with pre-configure logging:

1. Create any `ILogger` instance:

```cs
ILogger logger = ...;
```

2. via [IHttpClientFactory](xref:Softeq.XToolkit.Remote.Client.IHttpClientFactory):

```cs
var httpClientFactory = new DefaultHttpClientFactory();

var httpClient = httpClientFactory.CreateClient("https://softeq.com", logger);
```

2. via [HttpClientBuilder](xref:Softeq.XToolkit.Remote.Client.HttpClientBuilder):

```cs
var httpClient = new HttpClientBuilder()
    .WithBaseUrl("https://softeq.com")
    .WithLogger(logger, LogVerbosity.All)
    .Build();
```

> By default `.WithLogger()` will be use `HttpDiagnosticsHandler`.

> You can set the verbosity for all of your `HttpDiagnosticsHandler` instances by setting `HttpDiagnosticsHandler.DefaultVerbosity`. To set verbosity at the per-instance level, use `HttpDiagnosticsHandler` constructor which will override `HttpDiagnosticsHandler.DefaultVerbosity`.

3. Use configured HttpClient:

```cs
var remoteService = remoteServiceFactory.Create<IApi>(httpClient);
```

### Request options

```cs
var options = new RequestOptions
{
    Timeout = 5,
    RetryCount = 2
};

var result = await remoteService.MakeRequest(
    (s, ct) => s.GetProfile(ct),
    options);
```

## Examples

- [Playground.Forms.Remote](https://github.com/Softeq/XToolkit.WhiteLabel/tree/master/samples/Playground.Forms/Playground.Forms/Remote)
  - Softeq Auth sample
  - POST forms
  - Upload data
  - Stream data deserialize
  - FFImageLoading integration
  - etc.

---
