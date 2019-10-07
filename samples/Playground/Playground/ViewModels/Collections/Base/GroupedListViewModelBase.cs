// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Playground.Services;
using Playground.ViewModels.Collections.Products;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Playground.ViewModels.Collections.Base
{
    public abstract class GroupedListViewModelBase : ViewModelBase
    {
        private readonly IDialogsService _dialogsService;
        private readonly ICommand<ProductViewModel> _addToCartCommand;
        private readonly ICommand<ProductHeaderViewModel> _addGroupToCartCommand;
        private readonly ICommand<ProductHeaderViewModel> _groupInfoCommand;
        private readonly ICommand<ProductHeaderViewModel> _generateItemCommand;

        public GroupedListViewModelBase(
            IDialogsService dialogsService,
            IDataService dataService)
        {
            _dialogsService = dialogsService;
            AddAllToCartCommand = new RelayCommand(AddAll);
            GenerateGroupCommand = new RelayCommand(GenerateGroup);
            _addGroupToCartCommand = new RelayCommand<ProductHeaderViewModel>(AddGroupToBasket);
            _generateItemCommand = new RelayCommand<ProductHeaderViewModel>(GenerateItem);
            _groupInfoCommand = new AsyncCommand<ProductHeaderViewModel>(GroupInfo);
            _addToCartCommand = new RelayCommand<ProductViewModel>(AddToBasket, CanAddToBasket);
            ProductListViewModel = new ProductListViewModel(dataService, _addGroupToCartCommand, _generateItemCommand, _addToCartCommand, _groupInfoCommand);
            ProductBasketViewModel = new ProductBasketViewModel();
        }

        public ICommand AddAllToCartCommand { get; }

        public ICommand GenerateGroupCommand { get; }

        public ProductListViewModel ProductListViewModel { get; }

        public ProductBasketViewModel ProductBasketViewModel { get; }

        public override async void OnInitialize()
        {
            base.OnInitialize();

            await ProductListViewModel.LoadDataAsync();
        }

        private void AddAll()
        {
            ProductListViewModel.Products
                .SelectMany(x => x)
                .Apply(ProductBasketViewModel.AddItem);

            ProductListViewModel.ClearGroups();
        }

        private void GenerateGroup()
        {
            ProductListViewModel.TryGenerateGroup();
        }

        private void AddGroupToBasket(ProductHeaderViewModel viewModel)
        {
            ProductListViewModel.Products
                .First(x => x.Key.Equals(viewModel))
                .Apply(x => ProductBasketViewModel.AddItem(x));

            ProductListViewModel.ClearGroup(viewModel);
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

        private void GenerateItem(ProductHeaderViewModel groupHeader)
        {
            ProductListViewModel.GenerateAndAddItem(groupHeader);
        }
    }
}
