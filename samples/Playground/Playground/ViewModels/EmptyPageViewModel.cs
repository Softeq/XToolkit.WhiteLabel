// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.HttpBin;
using Playground.RemoteData.HttpBin.Dtos;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.Remote.Api;
using Softeq.XToolkit.Remote.Client;
using Softeq.XToolkit.Remote.Primitives;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels
{
    [SuppressMessage("ReSharper", "EmptyDestructor", Justification = "Just for play.")]
    [SuppressMessage("ReSharper", "RedundantOverriddenMember", Justification = "Just for play.")]
    [SuppressMessage("ReSharper", "EmptyConstructor", Justification = "Just for play.")]
    public class EmptyPageViewModel : ViewModelBase
    {
        private readonly HttpMessageHandler _customHttpMessageHandler;

        public EmptyPageViewModel(
            HttpMessageHandler customHttpMessageHandler)
        {
            _customHttpMessageHandler = customHttpMessageHandler;
            // Put your code HERE.
        }

        ~EmptyPageViewModel()
        {
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            // Put your code HERE.
            Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromSeconds(1));

                try
                {
                    var customHttpMessageHandlerBuilder = new DefaultHttpMessageHandlerBuilder(_customHttpMessageHandler);

                    var httpClient = new HttpClientBuilder(customHttpMessageHandlerBuilder)
                        .WithBaseUrl("https://httpbin.org")
                        .WithLogger(new ConsoleLogger("XHTTP"), LogVerbosity.All)
                        .AddHandler(new TestRetryHandler())
                        .Build();

                    var apiServiceFactory = new RefitApiServiceFactory();
                    var apiService = apiServiceFactory.CreateService<IHttpBinApiService>(httpClient);

                    var content = new PostContent { Name = "TestName", Age = 123 };
                    var response = await apiService.PostRawRequestAsync("test_parameter", content, CancellationToken.None);
                    Console.WriteLine(response);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            });
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            // Put your code HERE.
        }
    }

    public class TestRetryHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            var request1 = request;
            var request2 = HttpRequestMessageExtensions.Clone(request);

            var response1 = await base.SendAsync(request1, cancellationToken);

            await Task.Delay(TimeSpan.FromSeconds(1));

            var response2 = await base.SendAsync(request1, cancellationToken);
            return response2;
        }
    }

    public static class HttpRequestMessageExtensions
    {
        public static HttpRequestMessage Clone(this HttpRequestMessage req)
        {
            HttpRequestMessage clone = new HttpRequestMessage(req.Method, req.RequestUri);

            clone.Content = Copy(req.Content);
            clone.Version = req.Version;

            foreach (KeyValuePair<string, object> prop in req.Properties)
            {
                clone.Properties.Add(prop);
            }

            foreach (KeyValuePair<string, IEnumerable<string>> header in req.Headers)
            {
                clone.Headers.TryAddWithoutValidation(header.Key, header.Value);
            }

            return clone;
        }

        private static HttpContent Copy(HttpContent content)
        {
            return new CustomPushStreamContent(
                async (stream, _, __) =>
                {
                    using (stream)
                    {
                        await content.CopyToAsync(stream).ConfigureAwait(false);
                    }
                }, content.Headers.ContentType);
        }
    }
}
