// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using System.Threading.Tasks;
using Playground.Services;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Collections.Products
{
    public class ProductListViewModel : ViewModelBase
    {
        private readonly IDataService _dataService;
        private readonly ICommand<ProductViewModel> _addToCartCommand;
        private readonly ICommand<ProductHeaderViewModel> _groupInfoCommand;

        public ProductListViewModel(
            IDataService dataService,
            ICommand<ProductViewModel> addToCartCommand,
            ICommand<ProductHeaderViewModel> groupInfoCommand)
        {
            _dataService = dataService;
            _addToCartCommand = addToCartCommand;
            _groupInfoCommand = groupInfoCommand;

            Products = new ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel>();
        }

        public ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel> Products { get; }

        public async Task LoadDataAsync()
        {
            IsBusy = true;

            var products = await _dataService.GetProducts(40);

            products.Apply(product =>
            {
                product.AddToCartCommand = _addToCartCommand;
            });

            Products.AddRangeToGroups(products, ToGroup);

            IsBusy = false;
        }

        public void RemoveItem(ProductViewModel productViewModel)
        {
            Products.RemoveAllFromGroups(productViewModel);
        }

        private ProductHeaderViewModel ToGroup(ProductViewModel product)
        {
            return new ProductHeaderViewModel
            {
                Category = product.Title.First().ToString(),
                InfoCommand = _groupInfoCommand
            };
        }
    }
}
