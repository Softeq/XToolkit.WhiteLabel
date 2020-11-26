// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Diagnostics;
using System.Threading.Tasks;
using Playground.Forms.Remote.Services;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.Remote.ViewModels
{
    public class RemotePageViewModel : ViewModelBase
    {
        private readonly RemoteDataService _remoteDataService;

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
                var result = await _remoteDataService.TestRequestAsync();

                Debug.WriteLine(result);
            });
        }
    }
}
