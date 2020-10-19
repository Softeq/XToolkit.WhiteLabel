// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.ViewModels.Tab
{
    public class ToolbarViewModelBaseTests
    {
        private readonly ToolbarViewModelBase<string> _viewModel;

        public ToolbarViewModelBaseTests()
        {
            _viewModel = new ToolbarViewModelBaseStub();
        }

        [Fact]
        public void Ctor_DefaultSelectedIndex_ReturnsZero()
        {
            Assert.Equal(0, _viewModel.SelectedIndex);
        }

        [Fact]
        public void Ctor_DefaultTabModels_ReturnsEmpty()
        {
            Assert.Empty(_viewModel.TabModels);
        }

        [Fact]
        public void Ctor_DefaultTabViewModels_ReturnsEmpty()
        {
            Assert.Empty(_viewModel.TabViewModels);
        }

        [Fact]
        public void OnInitialize_Default_ExecutesCorrectly()
        {
            _viewModel.OnInitialize();
        }

        [Fact]
        public void OnInitialize_WithTabModels_InitializeViewModels()
        {
            var container = Substitute.For<IContainer>();
            var tabs = new List<TabItem<string>>
            {
                new TabItem<ViewModelStub, string>("title1", "key1", container),
                new TabItem<ViewModelStub, string>("title2", "key2", container)
            };
            ((ToolbarViewModelBaseStub)_viewModel).PublicSetTabModels(tabs);

            _viewModel.OnInitialize();

            Assert.Equal(tabs.Count, _viewModel.TabViewModels.Count);
        }
    }
}
