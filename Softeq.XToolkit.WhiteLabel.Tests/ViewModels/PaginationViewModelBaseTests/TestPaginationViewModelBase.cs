// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.ViewModels;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.PaginationViewModelBaseTests
{
    internal class TestPaginationViewModelBase : PaginationViewModelBase<string, string>
    {
        public int PublicPageSize { get; set; }
        public int TotalItemsCount { get; set; }

        protected override int PageSize => PublicPageSize;

        public async Task PublicLoadFirstPageAsync(CancellationToken ct = default)
        {
            await LoadFirstPageAsync(ct);
        }

        protected override IList<string> MapItemsToViewModels(IList<string> models)
        {
            return models;
        }

        protected override Task<PagingModel<string>> LoadAsync(int pageNumber, int pageSize)
        {
            return Task.FromResult(MockLoadAsync(pageNumber, pageSize));
        }

        private PagingModel<string> MockLoadAsync(int pageNumber, int pageSize)
        {
            var data = Enumerable
                .Range(0, TotalItemsCount)
                .Skip(pageNumber * pageSize)
                .Take(pageSize)
                .Select(x => x.ToString()).ToList();

            var model = new PagingModel<string>
            {
                Page = pageNumber,
                PageSize = data.Count,
                Data = data,
                TotalNumberOfPages = (int) Math.Ceiling(TotalItemsCount / (double)pageSize),
                TotalNumberOfRecords = TotalItemsCount
            };
            return model;
        }
    }
}
