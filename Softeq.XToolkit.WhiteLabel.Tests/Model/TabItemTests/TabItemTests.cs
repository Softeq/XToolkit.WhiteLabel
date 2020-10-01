// Developed by Softeq Development Corporation
// http://www.softeq.com

using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Model.TabItemTests
{
    public class TabItemTests
    {
        [Theory]
        [CombinatorialData]
        public void Ctor_WithViewModelAndKey_InitializesProperties(
            [CombinatorialValues(null, "")] string title,
            [CombinatorialValues(0, 1)] int key)
        {
            var container = Substitute.For<IContainer>();
            var tabItem = new TabItem<ViewModelStub, int>(title, key, container);

            Assert.Equal(title, tabItem.Title);
            Assert.Equal(key, tabItem.Key);
        }

        [Theory]
        [CombinatorialData]
        public void CreateViewModel_CreatesAndInitializesViewModelCorrectly(
            [CombinatorialValues(null, "")] string title,
            [CombinatorialValues(0, 1)] int key)
        {
            var container = Substitute.For<IContainer>();
            var frameNavigationService = Substitute.For<IFrameNavigationService>();
            container.Resolve<IFrameNavigationService>().ReturnsForAnyArgs(frameNavigationService);

            var tabItem = new TabItem<ViewModelStub, int>(title, key, container);
            var viewModel = tabItem.CreateViewModel();

            Assert.NotNull(viewModel);
            Assert.IsAssignableFrom<TabViewModel<ViewModelStub, int>>(viewModel);
            Assert.Equal(frameNavigationService, viewModel.FrameNavigationService);
            Assert.Equal(title, viewModel.Title);
            Assert.Equal(key, viewModel.Key);
        }
    }
}
