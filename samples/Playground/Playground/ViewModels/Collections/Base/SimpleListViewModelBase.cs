// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Playground.Extended;
using Playground.Models;
using Playground.Services;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Dialogs;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Collections.Base
{
    public abstract class SimpleListViewModelBase : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly IExtendedDialogsService _dialogsService;

        public SimpleListViewModelBase(
            IDataService dataService,
            IExtendedDialogsService dialogsService)
        {
            _dataService = dataService;
            _dialogsService = dialogsService;

            ItemModels = new ObservableRangeCollection<ItemViewModel>();

            SelectItemCommand = new AsyncCommand<ItemViewModel>(SelectItem);
        }

        public ObservableRangeCollection<ItemViewModel> ItemModels { get; }

        public ICommand<ItemViewModel> SelectItemCommand { get; }

        public override void OnInitialize()
        {
            base.OnInitialize();

            DoWorkAsync().FireAndForget(ex =>
            {
                _dialogsService.ShowDialogAsync(new AlertDialogConfig("Error", ex.Message, "OK"));
            });
        }

        private async Task SelectItem(ItemViewModel viewModel)
        {
            await _dialogsService.ShowDialogAsync(new AlertDialogConfig("Selected", viewModel.Title, "OK"));
        }

        private async Task DoWorkAsync()
        {
            await Task.Delay(1000);

            var items = _dataService.GetItems();

            ItemModels.AddRange(items);

            for (var i = 0; i < 50; i++)
            {
                var number = i.ToString();
                ItemModels.Apply(x => x.Title += number);

                await Task.Delay(1000);
            }
        }
    }
}
