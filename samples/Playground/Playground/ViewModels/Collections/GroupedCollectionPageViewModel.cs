// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Collections;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Collections
{
    public class GroupedCollectionPageViewModel : ViewModelBase
    {
        public GroupedCollectionPageViewModel()
        {
            Products = new ObservableRangeCollection<ProductItemViewModel>();
            Bag = new ObservableRangeCollection<ProductModel>();
        }

        public ObservableRangeCollection<ProductItemViewModel> Products { get; }

        public ObservableRangeCollection<ProductModel> Bag { get; }

        public string Title => $"Cart: {Bag.Count}";

        public override async void OnInitialize()
        {
            base.OnInitialize();

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            await Task.Delay(2000);

            var productModels = GenerateProducts(100);

            var productViewModels = productModels.Select(product => new ProductItemViewModel(product)
            {
                AddToCartCommand = new ContextableCommand<ProductModel>(AddToCart, product)
            });

            Products.AddRange(productViewModels);
        }

        private IEnumerable<ProductModel> GenerateProducts(int count)
        {
            return Enumerable.Range(0, count).Select(i => new ProductModel
            {
                Title = $"{i} #- Title",
                PhotoUrl = $"https://picsum.photos/100/150?random={i}",
                Price = 0
            });
        }

        private void AddToCart(ProductModel product)
        {
            Bag.Add(product);
            RaisePropertyChanged(() => Title);

            var productViewModel = Products.First(x => x.Title == product.Title);
            Products.Remove(productViewModel);
        }
    }

    // TODO YP: temporary
    public class ContextableCommand<T> : ICommand
    {
        private readonly WeakAction<T> _command;
        private readonly T _dataContext;

        public event EventHandler CanExecuteChanged;

        public ContextableCommand(Action<T> execute, T dataContext)
        {
            _command = new WeakAction<T>(execute);
            _dataContext = dataContext;
        }

        public bool CanExecute(object parameter)
        {
            return true;
        }

        public void Execute(object parameter)
        {
            if (_command.IsStatic || _command.IsAlive)
            {
                _command.Execute(_dataContext);
            }
        }
    }

    public class ProductItemViewModel : ObservableObject
    {
        private string _title;
        private string _photoUrl;

        public ProductItemViewModel(ProductModel product)
        {
            _title = product.Title;
            _photoUrl = product.PhotoUrl;
        }

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        public string PhotoUrl
        {
            get => _photoUrl;
            set => Set(ref _photoUrl, value);
        }

        public ICommand AddToCartCommand { get; set; }
    }

    public class ProductModel
    {
        public string Title { get; set; }
        public string PhotoUrl { get; set; }
        public double Price { get; set; }
    }
}
