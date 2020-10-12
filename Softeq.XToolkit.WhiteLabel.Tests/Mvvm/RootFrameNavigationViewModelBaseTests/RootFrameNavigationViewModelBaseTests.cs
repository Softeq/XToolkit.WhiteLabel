// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Mvvm.RootFrameNavigationViewModelBaseTests
{
    public class RootFrameNavigationViewModelBaseTests
    {
        private readonly IFrameNavigationService _frameNavigationService;
        private readonly RootFrameNavigationViewModelBaseStub _vm;

        public RootFrameNavigationViewModelBaseTests()
        {
            _frameNavigationService = Substitute.For<IFrameNavigationService>();
            _vm = new RootFrameNavigationViewModelBaseStub(_frameNavigationService);
        }

        [Fact]
        public void RootFrameNavigationViewModelBase_IsViewModelBase()
        {
            Assert.IsAssignableFrom<IViewModelBase>(_vm);
        }

        [Fact]
        public void Ctor_Null_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => new RootFrameNavigationViewModelBaseStub(null));
        }

        [Fact]
        public void Ctor_NotNull_InitializesProperties()
        {
            Assert.Same(_frameNavigationService, _vm.FrameNavigationService);
            Assert.False(_vm.IsBusy);
        }

        [Fact]
        public void InitializeNavigation_InitializesNavigationService()
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
