// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.BackStackManagerTests
{
    public class BackStackManagerTests
    {
        private readonly BackStackManager _backStackManager;

        public BackStackManagerTests()
        {
            _backStackManager = new BackStackManager();
        }

        [Fact]
        public void Ctor_InitializesEmptyBackStack()
        {
            Assert.Equal(0, _backStackManager.Count);
        }

        [Theory]
        [MemberData(
            nameof(BackStackManagerTestsDataProvider.PushViewModelTestData),
            MemberType = typeof(BackStackManagerTestsDataProvider))]
        public void PushViewModel_AddsViewModelToBackStack(
            List<IViewModelBase> initialViewModels,
            IViewModelBase viewModel)
        {
            InitializeBackStack(initialViewModels);
            _backStackManager.PushViewModel(viewModel);

            Assert.Equal(initialViewModels.Count + 1, _backStackManager.Count);
        }

        [Fact]
        public void PopViewModel_WithEmptyBackStack_ThrowsCorrectException()
        {
            Assert.Throws<InvalidOperationException>(() => _backStackManager.PopViewModel());
        }

        [Theory]
        [MemberData(
            nameof(BackStackManagerTestsDataProvider.PopAndPeekViewModelTestsData),
            MemberType = typeof(BackStackManagerTestsDataProvider))]
        public void PopViewModel_WithNonEmptyBackStack_ReturnsAndRemovesLastAddedViewModel(
            List<IViewModelBase> viewModels, IViewModelBase expectedResult)
        {
            InitializeBackStack(viewModels);

            var result = _backStackManager.PopViewModel();

            Assert.Equal(viewModels.Count - 1, _backStackManager.Count);
            Assert.Same(expectedResult, result);
        }

        [Fact]
        public void TryPopViewModel_WithEmptyBackStack_ReturnsFalse()
        {
            IViewModelBase result = new ViewModelStub();

            var popped = _backStackManager.TryPopViewModel(out result);

            Assert.False(popped);
            Assert.Null(result);
        }

        [Theory]
        [MemberData(
            nameof(BackStackManagerTestsDataProvider.PopAndPeekViewModelTestsData),
            MemberType = typeof(BackStackManagerTestsDataProvider))]
        public void TryPopViewModel_WithNonEmptyBackStack_ReturnsTrueAndViewModelAndRemoves(
            List<IViewModelBase> viewModels, IViewModelBase expectedResult)
        {
            InitializeBackStack(viewModels);

            IViewModelBase result = new ViewModelStub();

            var popped = _backStackManager.TryPopViewModel(out result);

            Assert.True(popped);
            Assert.Equal(viewModels.Count - 1, _backStackManager.Count);
            Assert.Same(expectedResult, result);
        }

        [Fact]
        public void PeekViewModel_WithEmptyBackStack_ThrowsCorrectException()
        {
            Assert.Throws<InvalidOperationException>(() => _backStackManager.PeekViewModel());
        }

        [Theory]
        [MemberData(
            nameof(BackStackManagerTestsDataProvider.PopAndPeekViewModelTestsData),
            MemberType = typeof(BackStackManagerTestsDataProvider))]
        public void PeekViewModel_WithNonEmptyBackStack_ReturnsLastAddedViewModelWithoutRemoval(
            List<IViewModelBase> viewModels, IViewModelBase expectedResult)
        {
            InitializeBackStack(viewModels);

            var result = _backStackManager.PeekViewModel();

            Assert.Equal(viewModels.Count, _backStackManager.Count);
            Assert.Same(expectedResult, result);
        }

        [Fact]
        public void TryPeekViewModel_WithEmptyBackStack_ReturnsFalse()
        {
            IViewModelBase result = new ViewModelStub();

            var peeked = _backStackManager.TryPeekViewModel(out result);

            Assert.False(peeked);
            Assert.Null(result);
        }

        [Theory]
        [MemberData(
            nameof(BackStackManagerTestsDataProvider.PopAndPeekViewModelTestsData),
            MemberType = typeof(BackStackManagerTestsDataProvider))]
        public void TryPeekViewModel_WithNonEmptyBackStack_ReturnsTrueAndViewModelWithoutRemoval(
            List<IViewModelBase> viewModels, IViewModelBase expectedResult)
        {
            InitializeBackStack(viewModels);

            IViewModelBase result = new ViewModelStub();

            var peeked = _backStackManager.TryPeekViewModel(out result);

            Assert.True(peeked);
            Assert.Equal(viewModels.Count, _backStackManager.Count);
            Assert.Same(expectedResult, result);
        }

        [Theory]
        [MemberData(
            nameof(BackStackManagerTestsDataProvider.ClearTestData),
            MemberType = typeof(BackStackManagerTestsDataProvider))]
        public void Clear_ClearsBackStack(
            List<IViewModelBase> viewModels)
        {
            InitializeBackStack(viewModels);

            _backStackManager.Clear();

            Assert.Equal(0, _backStackManager.Count);
        }

        private void InitializeBackStack(List<IViewModelBase> viewModels)
        {
            foreach (var vm in viewModels)
            {
                _backStackManager.PushViewModel(vm);
            }
        }
    }
}
