// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.DialogsServiceExtensionsTests
{
    public class DialogsServiceExtensionsTests
    {
        [Fact]
        public void For_WhenCalledOnNull_ThrowsCorrectException()
        {
            IDialogsService dialogsService = null;

            Assert.Throws<ArgumentNullException>(() => dialogsService.For<DialogViewModelStub>());
        }

        [Fact]
        public void For_WhenCalledOnNonNull_CreatesFluentNavigator()
        {
            var dialogsService = Substitute.For<IDialogsService>();

            var fluentNavigator = dialogsService.For<DialogViewModelStub>();

            Assert.NotNull(fluentNavigator);
        }
    }
}
