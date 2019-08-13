// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Playground.ViewModels.Collections.Products;

namespace Playground.Services
{
    public interface IDataService
    {
        Task<IList<ProductViewModel>> GetProducts(int count);
    }
}
