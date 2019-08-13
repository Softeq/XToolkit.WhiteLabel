// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Playground.Services;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Collections
{
    public class GroupedCollectionPageViewModel : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;
        private readonly IDataService _dataService;
        private readonly ICommand<ProductViewModel> _addToCartCommand;
        private readonly ICommand<ProductHeaderViewModel> _groupInfoCommand;

        public GroupedCollectionPageViewModel(
            IDialogsService dialogsService)
        {
            _dataService = new DataService();
            _addToCartCommand = new RelayCommand<ProductViewModel>(AddToCart, CanToAdd);
            _groupInfoCommand = new RelayCommand<ProductHeaderViewModel>(GroupInfo);

            ProductListViewModel = new ProductListViewModel(_dataService, _addToCartCommand, _groupInfoCommand);
            ProductBagViewModel = new ProductBagViewModel();
            _dialogsService = dialogsService;
        }

        public ICommand<ProductViewModel> AddToCartCommand { get; }

        public ProductListViewModel ProductListViewModel { get; }

        public ProductBagViewModel ProductBagViewModel { get; }

        public override async void OnInitialize()
        {
            base.OnInitialize();

            await ProductListViewModel.LoadDataAsync();
        }

        private void AddToCart(ProductViewModel product)
        {
            ProductBagViewModel.AddItem(product);
            ProductListViewModel.RemoveItem(product);
        }

        private bool CanToAdd(ProductViewModel product)
        {
            return product.Count > 0;
        }

        private async void GroupInfo(ProductHeaderViewModel groupHeader)
        {
            await _dialogsService.ShowDialogAsync("Info", $"{groupHeader.Category}th section.", "OK");
        }
    }
}
