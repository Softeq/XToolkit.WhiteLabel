using System;
using System.Threading.Tasks;
using Playground.Services;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Collections
{
    public class GroupedTablePageViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;
        private readonly ICommand<ProductHeaderViewModel> _groupInfoCommand;
        private readonly ICommand<ProductHeaderViewModel> _generateItemCommand;

        public GroupedTablePageViewModel(
            IDialogsService dialogsService,
            IDataService dataService)
        {
            _dialogsService = dialogsService;
            _generateItemCommand = new AsyncCommand<ProductHeaderViewModel>(GenerateItem);
            _groupInfoCommand = new AsyncCommand<ProductHeaderViewModel>(GroupInfo);
            AddToCartCommand = new RelayCommand<ProductViewModel>(AddToBasket, CanAddToBasket);
            ProductListViewModel = new ProductListViewModel(dataService, _generateItemCommand, AddToCartCommand, _groupInfoCommand);
            ProductBasketViewModel = new ProductBasketViewModel();
        }

        public ICommand<ProductViewModel> AddToCartCommand { get; }

        public ProductListViewModel ProductListViewModel { get; }

        public ProductBasketViewModel ProductBasketViewModel { get; }

        public override async void OnInitialize()
        {
            base.OnInitialize();

            await ProductListViewModel.LoadDataAsync();
        }

        private void AddToBasket(ProductViewModel product)
        {
            ProductBasketViewModel.AddItem(product);
            ProductListViewModel.RemoveItem(product);
        }

        private bool CanAddToBasket(ProductViewModel product)
        {
            return true;
            //return product.Count > 0;
        }

        private async Task GroupInfo(ProductHeaderViewModel groupHeader)
        {
            await _dialogsService.ShowDialogAsync("Info", $"{groupHeader.Category}th section.", "OK");
        }

        private Task GenerateItem(ProductHeaderViewModel groupHeader)
        {
            return ProductListViewModel.GenerateItem(groupHeader);
        }
    }
}
