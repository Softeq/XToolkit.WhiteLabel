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

        protected override async Task<PagingModel<string>> LoadAsync(string? query, int pageIndex, int pageSize)
        {
            await Task.Delay((pageIndex + 1) * 1000); // every page +1s delay

            var data = MockLoadData(query, pageIndex, pageSize);
            return data;
        }

        private PagingModel<string> MockLoadData(string? query, int pageIndex, int pageSize)
        {
            const int DefaultItemsCount = 30;

            var data = Enumerable
                .Range(0, DefaultItemsCount)
                .Skip(pageIndex * pageSize)
                .Take(pageSize)
                .Select(x => $"Page: {pageIndex} Item: {x} — {query}")
                .ToList();

            return new PagingModel<string>
            {
                Data = data,
                Page = pageIndex,
                PageSize = data.Count,
                TotalNumberOfPages = DefaultItemsCount / pageSize,
                TotalNumberOfRecords = DefaultItemsCount
            };
        }
    }
}
