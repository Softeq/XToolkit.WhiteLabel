// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.FrameNavigationServiceExtensionsTests
{
    public class FrameNavigationServiceExtensionsTests
    {
        [Fact]
        public void For_WhenCalledOnNull_ThrowsCorrectException()
        {
            IFrameNavigationService frameNavigationService = null;

            Assert.Throws<ArgumentNullException>(() => frameNavigationService.For<ViewModelStub>());
        }

        [Fact]
        public void For_WhenCalledOnNonNull_CreatesFluentNavigator()
        {
            var frameNavigationService = Substitute.For<IFrameNavigationService>();

            var fluentNavigator = frameNavigationService.For<ViewModelStub>();

            Assert.NotNull(fluentNavigator);
        }
    }
}
