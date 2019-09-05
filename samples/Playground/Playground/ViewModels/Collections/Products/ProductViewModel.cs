// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common.Command;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Playground.ViewModels.Collections.Products
{
    public class ProductViewModel : ObservableObject
    {
        private int _count;

        public string Title { get; set; }

        public string PhotoUrl { get; set; }

        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }

        public ICommand<ProductViewModel> AddToBasketCommand { get; set; }
    }
}
