// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Playground.RemoteData.HttpBin;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.Remote.ViewModels
{
    public class RemotePageViewModel : ViewModelBase
    {
        private readonly RemoteDataService _remoteDataService;

        private CancellationTokenSource? _cts;

        public RemotePageViewModel(
            RemoteDataService remoteDataService)
        {
            _remoteDataService = remoteDataService;
        }

        public override void OnAppearing()
        {
            base.OnAppearing();

            Task.Run(async () =>
            {
                try
                {
                    _cts = new CancellationTokenSource();

                    var result = await _remoteDataService.TestRequestAsync(_cts.Token);

                    Debug.WriteLine(result);
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            });
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            _cts?.Cancel();
        }
    }
}
