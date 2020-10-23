// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Threading.Tasks;
using Softeq.XToolkit.Common.Commands;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.PaginationViewModelBaseTests
{
    public class PaginationViewModelBaseTests
    {
        private const int DefaultCurrentPage = -1;

        private readonly TestPaginationViewModelBase _viewModel;

        public PaginationViewModelBaseTests()
        {
            _viewModel = new TestPaginationViewModelBase();

            Execute.Initialize(new MainThreadExecutorBaseStub());
        }

        [Fact]
        public void CurrentPage_Default_ReturnsNegative()
        {
            Assert.Equal(DefaultCurrentPage, _viewModel.CurrentPage);
        }

        [Fact]
        public void CanLoadMore_Default_ReturnsFalse()
        {
            Assert.False(_viewModel.CanLoadMore);
        }

        [Fact]
        public void LoadMoreCommand_Default_ReturnsIAsyncCommand()
        {
            Assert.IsAssignableFrom<IAsyncCommand>(_viewModel.LoadMoreCommand);
        }

        [Fact]
        public void Items_Default_ReturnsEmpty()
        {
            Assert.Empty(_viewModel.Items);
        }

        [Theory]
        [InlineData(3, 3)]
        [InlineData(12, 100)]
        public async Task LoadFirstPageAsync_TestData_LoadsItems(int pageSize, int totalItemsCount)
        {
            _viewModel.PublicPageSize = pageSize;
            _viewModel.TotalItemsCount = totalItemsCount;

            await _viewModel.PublicLoadFirstPageAsync();

            Assert.Equal(0, _viewModel.CurrentPage);
            Assert.Equal(pageSize, _viewModel.Items.Count);
        }

        [Fact]
        public async Task ResetItems_NotEmpty_Resets()
        {
            _viewModel.PublicPageSize = 2;
            _viewModel.TotalItemsCount = 2;
            await _viewModel.PublicLoadFirstPageAsync();

            _viewModel.ResetItems();

            Assert.Equal(DefaultCurrentPage, _viewModel.CurrentPage);
            Assert.Empty(_viewModel.Items);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(3, 2, 2)]
        public async Task LoadMoreCommand_SingleCallForNotEmptyItems_AddsItems(
            int totalItemsCount,
            int pageSize,
            int totalPagesCount)
        {
            _viewModel.PublicPageSize = pageSize;
            _viewModel.TotalItemsCount = totalItemsCount;
            await _viewModel.PublicLoadFirstPageAsync();

            await _viewModel.LoadMoreCommand.ExecuteAsync(null!);

            Assert.Equal(totalPagesCount - 1, _viewModel.CurrentPage);
            Assert.Equal(totalItemsCount, _viewModel.Items.Count);
        }

        [Theory]
        [InlineData(1, 1, 1)]
        [InlineData(2, 2, 1)]
        [InlineData(3, 2, 2)]
        [InlineData(4, 1, 4)]
        [InlineData(7, 2, 4)]
        public async Task LoadMoreCommand_MultipleCallsForNotEmptyItems_AddsItems(
            int totalItemsCount,
            int pageSize,
            int totalPagesCount)
        {
            _viewModel.PublicPageSize = pageSize;
            _viewModel.TotalItemsCount = totalItemsCount;
            await _viewModel.PublicLoadFirstPageAsync();

            var t1 = _viewModel.LoadMoreCommand.ExecuteAsync(null!);
            var t2 = _viewModel.LoadMoreCommand.ExecuteAsync(null!);
            var t3 = _viewModel.LoadMoreCommand.ExecuteAsync(null!);

            await Task.WhenAll(t1, t2, t3);

            Assert.Equal(totalPagesCount - 1, _viewModel.CurrentPage);
            Assert.Equal(totalItemsCount, _viewModel.Items.Count);
        }
    }
}
