// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.ViewModels;

namespace Playground.Forms.ViewModels.Components
{
    public class PaginationSearchPageViewModel : ViewModelBase
    {
        public PaginationSearchPageViewModel()
        {
            Search = new SearchViewModel();
        }

        public SearchViewModel Search { get; }
    }

    public class SearchViewModel : PaginationSearchViewModelBase<string, string>
    {
        protected override int PageSize => 10;

        protected override bool SilentLoadPagesEnabled => false;

        protected override IList<string> MapItemsToViewModels(IList<string> models)
        {
            return models;
        }

        protected override IList<string> FilterItems(IList<string> viewModels)
        {
            return base.FilterItems(viewModels);
        }

        protected override async Task<PagingModel<string>> LoadAsync(string? query, int pageNumber, int pageSize)
        {
            await Task.Delay((pageNumber + 1) * 1000); // every page +1s delay

            var data = await MockLoadData(query, pageNumber, pageSize);
            return data;
        }

        private async Task<PagingModel<string>> MockLoadData(string? query, int pageNumber, int pageSize)
        {
            const int DefaultItemsCount = 100;

            var startIndex = (pageNumber + 1) * pageSize;
            var isEndOfList = startIndex >= DefaultItemsCount;
            var items = isEndOfList
                ? new List<string>()
                : Enumerable
                    .Range(startIndex, pageSize)
                    .Select(x => $"Page: {pageNumber} Item: {x} — {query}")
                    .ToList();

            return new PagingModel<string>
            {
                Data = items,
                Page = pageNumber,
                PageSize = items.Count,
                TotalNumberOfPages = DefaultItemsCount / pageSize,
                TotalNumberOfRecords = DefaultItemsCount
            };
        }
    }
}
