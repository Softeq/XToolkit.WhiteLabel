// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.ViewModels;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.PaginationSearchViewModelBaseTests
{
    internal class TestPaginationSearchViewModelBaseTests : PaginationSearchViewModelBase<string, string>
    {
        private readonly Func<string, int, int, Task<PagingModel<string>>> _dataLoader;

        public TestPaginationSearchViewModelBaseTests(Func<string, int, int, Task<PagingModel<string>>> dataLoader)
        {
            _dataLoader = dataLoader;
        }

        public int TestSearchDelay => SearchDelay;

        protected override int SearchDelay => 50;

        protected override IList<string> MapItemsToViewModels(IList<string> models)
        {
            return models;
        }

        protected override Task<PagingModel<string>> LoadAsync(string query, int pageNumber, int pageSize)
        {
            return _dataLoader(query, pageNumber, pageSize);
        }
    }
}
