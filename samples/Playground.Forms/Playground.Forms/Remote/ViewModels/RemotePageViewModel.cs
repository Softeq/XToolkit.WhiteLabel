// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using System.Net.Http;
using System.Threading.Tasks;
using Playground.Forms.Remote.Services;
using Softeq.XToolkit.Remote;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.Remote.ViewModels
{
    public class RemotePageViewModel : ViewModelBase
    {
        public override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                var httpClient = new HttpClient
                {
                    BaseAddress = new Uri("https://postman-echo.com")
                };

                var remoteServiceFactory = new RemoteServiceFactory();

                var remoteService = remoteServiceFactory.Create<IPostmanEchoRemoteService>(httpClient);

                var result = await remoteService.MakeRequest((s, ct) => s.GetRequestAsync("foo1-value", "foo2-value"));

                Debug.WriteLine(result);
            });
        }
    }
}
