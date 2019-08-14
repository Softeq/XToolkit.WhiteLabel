// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Collections.Products
{
    public class ProductBasketViewModel : ObservableObject
    {
        private readonly IList<ProductViewModel> _products;

        public ProductBasketViewModel()
        {
            _products = new List<ProductViewModel>();
        }

        public string Status => $"Added: {_products.Count}; Total: {_products.Sum(x => x.Count)}";

        public void AddItem(ProductViewModel product)
        {
            _products.Add(product);

            RaisePropertyChanged(nameof(Status));
        }
    }
}
