// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Playground.Services;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Extensions;

namespace Playground.ViewModels.Collections.Products
{
    public class ProductListViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private readonly ICommand<ProductViewModel> _addCommand;
        private readonly ICommand<ProductHeaderViewModel> _infoCommand;
        private readonly ICommand<ProductHeaderViewModel> _generateCommand;
        private readonly ICommand<ProductHeaderViewModel> _addGroupToBasketCommand;

        private bool _isBusy;

        public ProductListViewModel(
            IDataService dataService,
            ICommand<ProductHeaderViewModel> addGroupToBasketCommand,
            ICommand<ProductHeaderViewModel> generateCommand,
            ICommand<ProductViewModel> addCommand,
            ICommand<ProductHeaderViewModel> infoCommand)
        {
            _addGroupToBasketCommand = addGroupToBasketCommand;
            _generateCommand = generateCommand;
            _dataService = dataService;
            _addCommand = addCommand;
            _infoCommand = infoCommand;

            Products = new ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel>();
        }

        public ObservableKeyGroupsCollection<ProductHeaderViewModel, ProductViewModel> Products { get; }

        public bool IsBusy
        {
            get => _isBusy;
            private set => Set(ref _isBusy, value);
        }

        public async Task LoadDataAsync()
        {
            IsBusy = true;

            var products = await _dataService.GetProducts(40);

            products.Apply(product => { product.AddToBasketCommand = _addCommand; });

            Products.ReplaceAllItems(products, x => CreateGroup(GetGroupId(x)), x => x);

            IsBusy = false;
        }

        public void TryGenerateGroup()
        {
            var id = GetNewGroupId();

            if (id > 9)
            {
                return;
            }

            var group = CreateGroup(id);

            var item = GenerateItem(group);

            Products.AddGroups(new List<KeyValuePair<ProductHeaderViewModel, IList<ProductViewModel>>>
            {
                new KeyValuePair<ProductHeaderViewModel, IList<ProductViewModel>>(group, new List<ProductViewModel> { item })
            });
        }

        public void GenerateAndAddItem(ProductHeaderViewModel productViewModel)
        {
            int newId = GetNewItemId(productViewModel);

            var newItem = _dataService.GetProduct(newId);
            newItem.AddToBasketCommand = _addCommand;

            Products.AddItems(
                new List<ProductViewModel> { GenerateItem(productViewModel) },
                x => CreateGroup(GetGroupId(x)),
                x => x);
        }

        private ProductViewModel GenerateItem(ProductHeaderViewModel productViewModel)
        {
            int newId = GetNewItemId(productViewModel);

            var newItem = _dataService.GetProduct(newId);
            newItem.AddToBasketCommand = _addCommand;

            return newItem;
        }

        private int GetNewGroupId()
        {
            return Products.Any()
                ? Products
                    .Select(x => x.Key)
                    .OrderBy(x => x.Id)
                    .Last()
                    .Id + 1
                : 1;
        }

        private int GetNewItemId(ProductHeaderViewModel productViewModel)
        {
            var groups = Products
                .FirstOrDefault(x => x.Key.Equals(productViewModel))?
                .ToList();

            if (groups == null
                || groups.Count == 0)
            {
                return int.Parse(productViewModel.Category);
            }

            int newId;

            int lastId = groups
                .OrderBy(x => x.Id)
                .Last()
                .Id;

            if ((lastId + 1).ToString()[0] == lastId.ToString()[0])
            {
                newId = lastId + 1;
            }
            else
            {
                int intLength = (int)Math.Floor(Math.Log10(lastId)) + 1;
                int mult = (int)Math.Pow(10, intLength);
                int coeff = lastId / (int)Math.Pow(10, intLength - 1);

                newId = coeff * mult;
            }

            return newId;
        }

        public void ClearGroups()
        {
            Products.Clear();
        }

        public void ClearGroup(ProductHeaderViewModel productHeaderViewModel)
        {
            Products.ClearGroup(productHeaderViewModel);
        }

        public void RemoveItem(ProductViewModel productViewModel)
        {
            Products.RemoveItems(new List<ProductViewModel> { productViewModel }, x => CreateGroup(GetGroupId(x)), x => x);
        }

        private ProductHeaderViewModel CreateGroup(int id)
        {
            return new ProductHeaderViewModel
            {
                Id = id,
                Category = id.ToString(),
                InfoCommand = _infoCommand,
                GenerateCommand = _generateCommand,
                AddCommand = _addGroupToBasketCommand
            };
        }

        private int GetGroupId(ProductViewModel product)
        {
            return int.Parse(product.Id.ToString()[0].ToString());
        }
    }
}
