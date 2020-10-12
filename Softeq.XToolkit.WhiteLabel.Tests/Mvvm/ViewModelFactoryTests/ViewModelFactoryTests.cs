// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Mvvm.ViewModelFactoryTests
{
    public class ViewModelFactoryTests
    {
        [Fact]
        public void Ctor_WithNullContainer_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => new ViewModelFactoryService(null));
        }

        [Fact]
        public void ResolveViewModel_ResolvesViewModelCorrectly()
        {
            var iocContainer = Substitute.For<IContainer>();
            iocContainer.Resolve<ViewModelStub>().ReturnsForAnyArgs(new ViewModelStub());

            var factoryService = new ViewModelFactoryService(iocContainer);

            var viewModel = factoryService.ResolveViewModel<ViewModelStub>();
            iocContainer.Received(1).Resolve<ViewModelStub>();

            Assert.NotNull(viewModel);
            Assert.IsAssignableFrom<ViewModelStub>(viewModel);
        }
    }
}
