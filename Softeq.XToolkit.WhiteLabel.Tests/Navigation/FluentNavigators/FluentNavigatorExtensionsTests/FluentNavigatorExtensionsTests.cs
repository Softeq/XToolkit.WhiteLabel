// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.FluentNavigators.FluentNavigatorExtensionsTests
{
    public class FluentNavigatorExtensionsTests
    {
        [Fact]
        public void ApplyParameters_CalledOnNull_WithNullParams_ThrowsCorrectException()
        {
            IViewModelBase viewModel = null;

            Assert.Throws<ArgumentNullException>(() => viewModel.ApplyParameters(null!));
        }

        [Fact]
        public void ApplyParameters_CalledOnNull_WithNonNullParams_ThrowsCorrectException()
        {
            IViewModelBase viewModel = null;
            var parameters = new List<NavigationParameterModel>();

            Assert.Throws<ArgumentNullException>(() => viewModel.ApplyParameters(parameters));
        }

        [Fact]
        public void ApplyParameters_CalledOnNonNull_WithNullParams_ThrowsCorrectException()
        {
            IViewModelBase viewModel = new ViewModelStub();

            Assert.Throws<ArgumentNullException>(() => viewModel.ApplyParameters(null!));
        }

        [Theory]
        [MemberData(
            nameof(FluentNavigatorExtensionsTestsDataProvider.InvalidNavigationParams),
            MemberType = typeof(FluentNavigatorExtensionsTestsDataProvider))]
        public void ApplyParameters_CalledOnNonNull_WithNonNullInvalidParams_ThrowsCorrectException(
            List<NavigationParameterModel> parameters)
        {
            IViewModelBase viewModel = new ViewModelStub();

            Assert.ThrowsAny<Exception>(() => viewModel.ApplyParameters(parameters));
        }

        [Fact]
        public void ApplyParameters_CalledOnNonNull_WithNonNullValidParams_SetsCorrectProperties()
        {
            var viewModel = new ViewModelStub();
            var parameters = FluentNavigatorExtensionsTestsDataProvider.ValidNavigationParams;

            viewModel.ApplyParameters(parameters);

            Assert.Equal(FluentNavigatorExtensionsTestsDataProvider.PropertyValue1, viewModel.StringParameter);
            Assert.Equal(FluentNavigatorExtensionsTestsDataProvider.PropertyValue2, viewModel.IntParameter);
        }
    }
}
