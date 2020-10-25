// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Input;
using NSubstitute;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Softeq.XToolkit.WhiteLabel.ViewModels;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.PaginationSearchViewModelBaseTests
{
    public class PaginationSearchViewModelBaseTests
    {
        private readonly Func<string, int, int, Task<PagingModel<string>>> _dataLoader;
        private readonly TestPaginationSearchViewModelBaseTests _viewModel;

        public PaginationSearchViewModelBaseTests()
        {
            _dataLoader = Substitute.For<Func<string, int, int, Task<PagingModel<string>>>>();
            _dataLoader
                .Invoke(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>())
                .Returns(Task.FromResult(new PagingModel<string>()));

            _viewModel = new TestPaginationSearchViewModelBaseTests(_dataLoader);

            Execute.Initialize(new MainThreadExecutorBaseStub());
        }

        private int TestSearchDelay => _viewModel.TestSearchDelay + 100;

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

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("  ")]
        public async Task SearchQuery_NullOrWhiteSpace_DoesNotLoadData(string input)
        {
            _viewModel.SearchQuery = input;

            await Task.Delay(TestSearchDelay);

            await _dataLoader
                .DidNotReceive()
                .Invoke(Arg.Any<string>(), Arg.Any<int>(), Arg.Any<int>());
        }

        [Theory]
        [InlineData("test")]
        [InlineData(" test")]
        [InlineData("test ")]
        public async Task SearchQuery_NotEmpty_DoesNotLoadData(string input)
        {
            _viewModel.SearchQuery = input;

            await Task.Delay(TestSearchDelay);

            await _dataLoader
                .Received()
                .Invoke(Arg.Is(input), Arg.Any<int>(), Arg.Any<int>());
        }

        [Fact]
        public async Task SearchQuery_MultipleConsecutiveQueries_LoadsOnlyLastSearchQuery()
        {
            const string FirstSearchQuery = "test1";
            const string LastSearchQuery = "test2";

            _viewModel.SearchQuery = FirstSearchQuery;
            _viewModel.SearchQuery = LastSearchQuery;

            await Task.Delay(TestSearchDelay);

            await _dataLoader
                .Received()
                .Invoke(Arg.Is(LastSearchQuery), Arg.Any<int>(), Arg.Any<int>());
        }
    }

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
