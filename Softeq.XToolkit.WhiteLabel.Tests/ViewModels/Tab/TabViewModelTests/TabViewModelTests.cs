// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.Tab.TabViewModelTests
{
    public class TabViewModelTests
    {
        private const string TestTabItemTitle = "test title";
        private const string TestTabItemKey = "test key";

        private readonly IFrameNavigationService _frameNavigationService;
        private readonly TabItem<string> _tabItem;
        private readonly TabViewModel<ViewModelStub, string> _viewModel;

        public TabViewModelTests()
        {
            _frameNavigationService = Substitute.For<IFrameNavigationService>();
            _tabItem = Substitute.For<TabItem<string>>(TestTabItemTitle, TestTabItemKey);

            _viewModel = new TabViewModel<ViewModelStub, string>(_frameNavigationService, _tabItem);
        }

        [Fact]
        public void Ctor_NullFragmentService_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TabViewModel<ViewModelStub, string>(null!, _tabItem);
            });
        }

        [Fact]
        public void Ctor_NullTabItem_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                new TabViewModel<ViewModelStub, string>(_frameNavigationService, null!);
            });
        }

        [Fact]
        public void Ctor_Default_ReturnsRootFrameNavigationViewModelBase()
        {
            Assert.IsAssignableFrom<RootFrameNavigationViewModelBase>(_viewModel);
        }

        [Fact]
        public void Ctor_BadgeText_ReturnsEmpty()
        {
            Assert.Null(_viewModel.BadgeText);
        }

        [Fact]
        public void Ctor_IsBadgeVisible_ReturnsFalse()
        {
            Assert.False(_viewModel.IsBadgeVisible);
        }

        [Fact]
        public void Ctor_Title_ReturnsTabItemTitle()
        {
            Assert.Equal(TestTabItemTitle, _viewModel.Title);
        }

        [Fact]
        public void Ctor_Key_ReturnsTabItemKey()
        {
            Assert.Equal(TestTabItemKey, _viewModel.Key);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void CanGoBack_WithFrameNavigationService_ReturnsFrameNavigationServiceCanGoBack(bool expectedCanGoBack)
        {
            _frameNavigationService.CanGoBack.Returns(expectedCanGoBack);

            var result = _viewModel.CanGoBack;

            Assert.Equal(expectedCanGoBack, result);
        }

        [Fact]
        public void GoBack_WithFrameNavigationService_ExecutesFrameNavigationServiceGoBack()
        {
            _viewModel.GoBack();

            _frameNavigationService.Received(1).GoBack();
        }
    }
}
