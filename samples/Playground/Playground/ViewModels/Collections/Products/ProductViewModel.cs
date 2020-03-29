// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.Common;
using Softeq.XToolkit.Common.Commands;

namespace Playground.ViewModels.Collections.Products
{
    public class ProductViewModel : ObservableObject
    {
        private int _count;

        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string PhotoUrl { get; set; } = string.Empty;

        public bool IsAddedToBasket { get; set; }

        public int Count
        {
            get => _count;
            set => Set(ref _count, value);
        }

        public ICommand<ProductViewModel>? AddToBasketCommand { get; set; }
    }
}
