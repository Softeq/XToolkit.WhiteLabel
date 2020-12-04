// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading;
using System.Windows.Input;
using Playground.Forms.Remote.Services;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.Forms.Remote.ViewModels
{
    public class RemotePageViewModel : ViewModelBase
    {
        private readonly RemoteDataService _remoteDataService;

        private CancellationTokenSource? _cts;

        public RemotePageViewModel(RemoteDataService remoteDataService)
        {
            _remoteDataService = remoteDataService;

            WorkItems = new ObservableRangeCollection<WorkItemViewModel>();

            RunAllCommand = new RelayCommand(() => WorkItems.Apply(x => x.RunCommand.Execute(null)));
            CancelAllCommand = new RelayCommand(() => WorkItems.Apply(x => x.CancelCommand.Execute(null)));
        }

        public ObservableRangeCollection<WorkItemViewModel> WorkItems { get; }

        public ICommand RunAllCommand { get; }

        public ICommand CancelAllCommand { get; }

        public override void OnAppearing()
        {
            base.OnAppearing();

            _cts = new CancellationTokenSource();

            var items = _remoteDataService.GetItems(_cts.Token);

            WorkItems.AddRange(items);
        }

        public override void OnDisappearing()
        {
            base.OnDisappearing();

            _cts?.Cancel();

            WorkItems.Clear();
        }
    }
}
