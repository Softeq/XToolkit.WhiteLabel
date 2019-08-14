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
    public class ProductListViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly ICommand<ProductViewModel> _addCommand;
        private readonly ICommand<ProductHeaderViewModel> _infoCommand;

        private bool _isBusy;

        public ProductListViewModel(
            IDataService dataService,
            ICommand<ProductViewModel> addCommand,
            ICommand<ProductHeaderViewModel> infoCommand)
        {
            _dataService = dataService;
            _addCommand = addCommand;
            _infoCommand = infoCommand;

            Products = new ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel>();
        }

        public ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel> Products { get; }

        public bool IsBusy
        {
            get => _isBusy;
            set => Set(ref _isBusy, value);
        }

        public async Task LoadDataAsync()
        {
            IsBusy = true;

            var products = await _dataService.GetProducts(40);

            products.Apply(product =>
            {
                product.AddToBasketCommand = _addCommand;
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
                InfoCommand = _infoCommand
            };
        }
    }
}
