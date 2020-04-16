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

You can install this component via NuGet: [![Softeq.XToolkit.Remote](https://buildstats.info/nuget/Softeq.XToolkit.Remote?includePreReleases=true)](https://www.nuget.org/packages/Softeq.XToolkit.Remote)

    Install-Package Softeq.XToolkit.Remote

## Usage

Below steps to describe a simple way to make an HTTP request.

### Step 1

Firstly you need to create an interface for declaring API:

```cs
public interface IApi
{
    [Get("/")]
    Task<string> GetHomePage();
}
```

> For declaring used [Refit](https://github.com/reactiveui/refit#api-attributes) library.

### Step 2

Create simple HttpClient:

```cs
var httpClient = new HttpClient
{
    BaseAddress = new Uri("https://google.com")
};
```

> The library provides a more advanced way to create HttpClient. See `DefaultHttpClientFactory`, `HttpClientBuilder`.

### Step 3

Create instance of RemoteService:

```cs
var remoteServiceFactory = new RemoteServiceFactory();
var remoteService = remoteServiceFactory.Create<IApi>(httpClient);
```

> In this case, can be used more simple way:
```cs
var remoteService = remoteServiceFactory.Create<IApi>("https://google.com");
```

### Step 4

Make simple request:

```cs
var result = await remoteService.MakeRequest((s, ct) => s.GetHomePage());
```

## Advanced

### Custom primary handler

Declare custom primary handler:

```cs
var customPrimaryHandler = new SocketsHttpHandler();
```

Use custom handler for HttpClientBuilder:

```cs
var httpMessageHandler = new DefaultHttpMessageHandlerBuilder(customPrimaryHandler);
var httpClientBuilder = new HttpClientBuilder("https://google.com", httpMessageHandler);
var httpClient = httpClientBuilder.Build();
```

### Setup HttpClient

> HttpClient <- [DefaultHttpClientFactory] <- HttpClientBuilder <- [DefaultHttpMessageHandlerBuilder] <- HttpMessageHandler

// TODO:

### Request options

```cs
var options = new RequestOptions
{
    Timeout = 5,
    RetryCount = 2
};
var result = await remoteService.MakeRequest((s, ct) => s.GetHomePage(), options);
```

### Enable logging

- Create HttpClient with pre-configure logging.

// TODO:
