// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Mvvm.RootFrameNavigationPageViewModelTests
{
    public class RootFrameNavigationPageViewModelTests
    {
        private readonly IFrameNavigationService _frameNavigationService;
        private readonly RootFrameNavigationPageViewModel<ViewModelStub> _vm;

        public RootFrameNavigationPageViewModelTests()
        {
            _frameNavigationService = Substitute.For<IFrameNavigationService>();
            _vm = new RootFrameNavigationPageViewModel<ViewModelStub>(_frameNavigationService);
        }

        [Fact]
        public void RootFrameNavigationPageViewModel_IsRootFrameNavigationViewModelBase()
        {
            Assert.IsAssignableFrom<RootFrameNavigationViewModelBase>(_vm);
        }

        [Fact]
        public void Ctor_Null_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => new RootFrameNavigationPageViewModel<ViewModelStub>(null));
        }

        [Fact]
        public void Ctor_NotNull_InitializesProperties()
        {
            Assert.Same(_frameNavigationService, _vm.FrameNavigationService);
            Assert.False(_vm.IsBusy);
        }

        [Fact]
        public async Task NavigateToFirstPageAsync_CallsNavigateAndClearsBackStack()
        {
            await _vm.NavigateToFirstPageAsync();

            await _frameNavigationService.Received(1).NavigateToViewModelAsync<ViewModelStub>(
                Arg.Is(true),
                Arg.Is((IReadOnlyList<NavigationParameterModel>) null));
        }

        [Fact]
        public void InitializeNavigation_Null_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => _vm.InitializeNavigation(null));
        }

        [Fact]
        public void InitializeNavigation_NotNull_InitializesNavigationService()
        {
            var param = new object();
            _vm.InitializeNavigation(param);

            _frameNavigationService.Received(1).Initialize(Arg.Is(param));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void IsInitialized_SameAsNavigationServiceIsInitialized(bool initialized)
        {
            _frameNavigationService.IsInitialized.Returns(initialized);

            Assert.Equal(initialized, _vm.IsInitialized);
        }
    }
}
