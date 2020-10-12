// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
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

            Assert.Throws<ArgumentNullException>(() => viewModel.ApplyParameters(null));
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

            Assert.Throws<ArgumentNullException>(() => viewModel.ApplyParameters(null));
        }

        [Fact]
        public void ApplyParameters_CalledOnNonNull_WithNonNullParams_SetsCorrectProperties()
        {
            var viewModel = new ViewModelStub();

            var propertyInfo1 = typeof(ViewModelStub).GetProperty(nameof(ViewModelStub.StringParameter));
            var propertyInfoModel1 = new PropertyInfoModel(propertyInfo1);
            var propertyValue1 = FluentNavigatorTestsDataProvider.NavigationParamValue;

            var propertyInfo2 = typeof(ViewModelStub).GetProperty(nameof(ViewModelStub.IntParameter));
            var propertyInfoModel2 = new PropertyInfoModel(propertyInfo2);
            var propertyValue2 = 5;

            var parameters = new List<NavigationParameterModel>
            {
                new NavigationParameterModel(propertyValue1, propertyInfoModel1),
                new NavigationParameterModel(propertyValue2, propertyInfoModel2),
            };

            viewModel.ApplyParameters(parameters);

            Assert.Equal(propertyValue1, viewModel.StringParameter);
            Assert.Equal(propertyValue2, viewModel.IntParameter);
        }
    }
}
