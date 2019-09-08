using System;
using System.Linq;
using System.Threading.Tasks;
using Playground.Services;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Collections
{
    public class GroupedTablePageViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;
        private readonly ICommand<ProductViewModel> _addToCartCommand;
        private readonly ICommand<ProductHeaderViewModel> _addGroupToBasketCommand;
        private readonly ICommand<ProductHeaderViewModel> _groupInfoCommand;
        private readonly ICommand<ProductHeaderViewModel> _generateItemCommand;

        public GroupedTablePageViewModel(
            IDialogsService dialogsService,
            IDataService dataService)
        {
            _dialogsService = dialogsService;
            _addGroupToBasketCommand = new RelayCommand<ProductHeaderViewModel>(AddGroupToBasket);
            _generateItemCommand = new AsyncCommand<ProductHeaderViewModel>(GenerateItem);
            _groupInfoCommand = new AsyncCommand<ProductHeaderViewModel>(GroupInfo);
            _addToCartCommand = new RelayCommand<ProductViewModel>(AddToBasket, CanAddToBasket);
            ProductListViewModel = new ProductListViewModel(dataService, _addGroupToBasketCommand, _generateItemCommand, _addToCartCommand, _groupInfoCommand);
            ProductBasketViewModel = new ProductBasketViewModel();
        }

        public ProductListViewModel ProductListViewModel { get; }

        public ProductBasketViewModel ProductBasketViewModel { get; }

        public override async void OnInitialize()
        {
            base.OnInitialize();

            await ProductListViewModel.LoadDataAsync();
        }

        private void AddGroupToBasket(ProductHeaderViewModel viewModel)
        {
            ProductListViewModel.Products
                .First(x => x.Key.Equals(viewModel))
                .Apply(x => ProductBasketViewModel.AddItem(x));

            ProductListViewModel.RemoveGroup(viewModel);
        }

        private void AddToBasket(ProductViewModel product)
        {
            product.IsAddedToBasket = true;
            ProductBasketViewModel.AddItem(product);
            ProductListViewModel.RemoveItem(product);
        }

        private bool CanAddToBasket(ProductViewModel product)
        {
            return !product.IsAddedToBasket;
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
