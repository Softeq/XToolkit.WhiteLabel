// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Mvvm.ViewModelFactoryTests
{
    public class ViewModelFactoryTests
    {
        private readonly IContainer _iocContainer;
        private readonly ViewModelFactoryService _factoryService;

        public ViewModelFactoryTests()
        {
            _iocContainer = Substitute.For<IContainer>();
            _iocContainer.Resolve<ViewModelStub>().ReturnsForAnyArgs(new ViewModelStub());
            _iocContainer.Resolve<ViewModelStub<string>>().ReturnsForAnyArgs(new ViewModelStub<string>());

            _factoryService = new ViewModelFactoryService(_iocContainer);
        }

        [Fact]
        public void Ctor_WithNullContainer_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => new ViewModelFactoryService(null));
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("abc")]
        public void ResolveViewModel_WithParamenter_ResolvesViewModelAndSetsParamenter(string param)
        {
            var viewModel = _factoryService.ResolveViewModel<ViewModelStub<string>, string>(param);
            _iocContainer.Received(1).Resolve<ViewModelStub<string>>();

            Assert.NotNull(viewModel);
            Assert.IsAssignableFrom<ViewModelStub<string>>(viewModel);
            Assert.IsAssignableFrom<IViewModelParameter<string>>(viewModel);
            Assert.Equal(viewModel.Parameter, param);
        }

        [Fact]
        public void ResolveViewModel_WithoutParamenter_ResolvesViewModel()
        {
            var viewModel = _factoryService.ResolveViewModel<ViewModelStub>();
            _iocContainer.Received(1).Resolve<ViewModelStub>();

            Assert.NotNull(viewModel);
            Assert.IsAssignableFrom<ViewModelStub>(viewModel);
        }
    }
}
