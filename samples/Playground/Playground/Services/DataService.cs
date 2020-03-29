// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Playground.Models;
using Playground.ViewModels.Collections.Products;

namespace Playground.Services
{
    public class DataService : IDataService
    {
        public async Task<IList<ProductViewModel>> GetProducts(int count)
        {
            await Task.Delay(1000);

            var productModels = GenerateProducts(count);
            return productModels.Select(Map).ToList();
        }

        public ProductViewModel GetProduct(int id)
        {
            return Map(GenerateProduct(id));
        }

        private ProductViewModel Map(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Title = product.Title,
                PhotoUrl = product.PhotoUrl
            };
        }

        private IEnumerable<Product> GenerateProducts(int count)
        {
            return Enumerable.Range(1, count).Select(GenerateProduct);
        }

        private Product GenerateProduct(int id)
        {
            return new Product
            {
                Id = id,
                Title = $"{id} #- Title",
                PhotoUrl = $"https://picsum.photos/100/150?random={id}",
                Price = id * 100
            };
        }
    }
}
