﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
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

            Products = new ObservableKeyGroupsCollectionNew<ProductHeaderViewModel, ProductViewModel>(false);
        }

        public ObservableKeyGroupsCollectionNew<ProductHeaderViewModel, ProductViewModel> Products { get; }

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

            Products.AddItems(products, ToGroup, x => x);

            IsBusy = false;
        }

        public async Task GenerateItem(ProductHeaderViewModel productViewModel)
        {
            int newId = await GetNewId(productViewModel);

            var newItem = _dataService.GetProduct(newId);
            newItem.AddToBasketCommand = _addCommand;

            Products.AddItems(new List<ProductViewModel> { newItem }, ToGroup, x => x);
        }

        private async Task<int> GetNewId(ProductHeaderViewModel productViewModel)
        {
            var products = await _dataService.GetProducts(40);

            var gr = Products
                .First(x => x.Key.Equals(productViewModel)).ToList();

            if(gr.Count == 0)
            {
                return int.Parse(productViewModel.Category);
            }

            int newId;

            int lastId = gr
                .OrderBy(x => x.Id)
                .Last()
                .Id;

            if ((lastId + 1).ToString()[0] == lastId.ToString()[0])
            {
                newId = lastId + 1;
            }
            else
            {
                int intLength = (int) Math.Floor(Math.Log10(lastId)) + 1;
                int mult = (int) Math.Pow(10, intLength);
                int coeff = (int) (lastId / (int) Math.Pow(10, intLength - 1));

                newId = (int) (coeff * mult);
            }

            return newId;
        }

        public void RemoveGroup(ProductHeaderViewModel productHeaderViewModel)
        {
            Products.RemoveGroups(new List<ProductHeaderViewModel> { productHeaderViewModel });
        }

        public void RemoveItem(ProductViewModel productViewModel)
        {
            Products.RemoveItems(new List<ProductViewModel> { productViewModel }, ToGroup, x => x);
        }

        private ProductHeaderViewModel ToGroup(ProductViewModel product)
        {
            return new ProductHeaderViewModel
            {
                Category = product.Title.First().ToString(),
                InfoCommand = _infoCommand,
                GenerateCommand = _generateCommand,
                AddCommand = _addGroupToBasketCommand
            };
        }
    }
}
