﻿// Developed by Softeq Development Corporation
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
            await Task.Delay(2000);

            var productModels = GenerateProducts(count);
            return productModels.Select(Map).ToList();
        }

        private ProductViewModel Map(Product product)
        {
            return new ProductViewModel
            {
                Title = product.Title,
                PhotoUrl = product.PhotoUrl
            };
        }

        private IEnumerable<Product> GenerateProducts(int count)
        {
            return Enumerable.Range(0, count).Select(i => new Product
            {
                Title = $"{i} #- Title",
                PhotoUrl = $"https://picsum.photos/100/150?random={i}",
                Price = i * 100
            });
        }
    }
}
