// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.ViewModels;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.PaginationSearchViewModelBaseTests
{
    public class PaginationSearchViewModelBaseTests
    {
        private readonly TestPaginationSearchViewModelBaseTests _viewModel;

        public PaginationSearchViewModelBaseTests()
        {
            _viewModel = new TestPaginationSearchViewModelBaseTests();
        }

        [Fact]
        public void ClearCommand_Default_ReturnsICommand()
        {
            Assert.IsAssignableFrom<ICommand>(_viewModel.ClearCommand);
        }

        [Fact]
        public void SearchCommand_Default_ReturnsICommand()
        {
            Assert.IsAssignableFrom<ICommand>(_viewModel.SearchCommand);
        }

        [Fact]
        public void SearchQuery_Default_ReturnsEmptyString()
        {
            Assert.Equal(string.Empty, _viewModel.SearchQuery);
        }

        [Fact]
        public void HasResults_Default_ReturnsFalse()
        {
            Assert.False(_viewModel.HasResults);
        }

        [Fact]
        public void IsBusy_Default_ReturnsFalse()
        {
            Assert.False(_viewModel.IsBusy);
        }
    }

    internal class TestPaginationSearchViewModelBaseTests : PaginationSearchViewModelBase<string, string>
    {
        protected override IList<string> MapItemsToViewModels(IList<string> models)
        {
            return models;
        }

        protected override Task<PagingModel<string>> LoadAsync(string query, int pageNumber, int pageSize)
        {
            throw new System.NotImplementedException();
        }
    }
}
