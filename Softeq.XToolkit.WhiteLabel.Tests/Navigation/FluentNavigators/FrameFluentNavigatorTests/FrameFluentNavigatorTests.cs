// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.FluentNavigators.FrameFluentNavigatorTests
{
    public class FrameFluentNavigatorTests
    {
        private readonly IFrameNavigationService _frameNavigationService;
        private readonly FrameFluentNavigator<ViewModelStub> _navigator;

        public FrameFluentNavigatorTests()
        {
            _frameNavigationService = Substitute.For<IFrameNavigationService>();
            _navigator = new FrameFluentNavigator<ViewModelStub>(_frameNavigationService);
        }

        [Fact]
        public void FrameFluentNavigator_IsFluentNavigatorBase()
        {
            Assert.IsAssignableFrom<FluentNavigatorBase<ViewModelStub>>(_navigator);
        }

        [Fact]
        public void Ctor_Null_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => new FrameFluentNavigator<ViewModelStub>(null));
        }

        [Fact]
        public void Initialize_Null_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => _navigator.Initialize(null));
        }

        [Fact]
        public void Initialize_NotNull_InitializesNavigationService()
        {
            var param = new object();
            _navigator.Initialize(param);

            _frameNavigationService.Received(1).Initialize(Arg.Is(param));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Navigate_NavigatesToTheSpecifiedViewModelWithSpecifiedParams(bool clearBackStack)
        {
            _navigator.NavigateAsync(clearBackStack);

            await _frameNavigationService.Received(1).NavigateToViewModelAsync<ViewModelStub>(
                Arg.Is(clearBackStack),
                Arg.Is(_navigator.Parameters));
        }

        [Fact]
        public void NavigateBack_NavigatesBack()
        {
            _navigator.NavigateBack();

            _frameNavigationService.Received(1).GoBackAsync();
        }

        [Fact]
        public void WithParams_ReturnsSelf()
        {
            var navParams = new List<NavigationParameterModel>
            {
                new NavigationParameterModel(default, null),
                new NavigationParameterModel(default, null),
                new NavigationParameterModel(default, null)
            };

            var newNavigator = _navigator.WithParams(navParams);

            Assert.Same(_navigator, newNavigator);
        }

        [Fact]
        public void WithParams_CalledWithNull_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => _navigator.WithParams(null));
        }

        [Theory]
        [MemberData(
            nameof(FluentNavigatorTestsDataProvider.NavigationParams),
            MemberType = typeof(FluentNavigatorTestsDataProvider))]
        public void WithParams_CalledOnce_AddsAllParameters(
            IEnumerable<NavigationParameterModel> navigationParameters)
        {
            _navigator.WithParams(navigationParameters);

            Assert.Equal(navigationParameters, _navigator.Parameters);
        }

        [Theory]
        [MemberData(
            nameof(FluentNavigatorTestsDataProvider.PairOfNavigationParams),
            MemberType = typeof(FluentNavigatorTestsDataProvider))]
        public void WithParams_CalledTwice_AddsParameters(
            IEnumerable<NavigationParameterModel> navigationParameters1,
            IEnumerable<NavigationParameterModel> navigationParameters2)
        {
            _navigator.WithParams(navigationParameters1);
            _navigator.WithParams(navigationParameters2);

            var emptyList = new List<NavigationParameterModel>();
            var result = new List<NavigationParameterModel>(navigationParameters1 ?? emptyList);
            result.AddRange(navigationParameters2 ?? emptyList);

            Assert.Equal(result, _navigator.Parameters);
        }

        [Fact]
        public void WithParam_ReturnsSelf()
        {
            var newNavigator = _navigator.WithParam(
                (vm) => vm.ObjectParameter,
                FluentNavigatorTestsDataProvider.NavigationParamValue);

            Assert.Same(_navigator, newNavigator);
        }

        [Fact]
        public void WithParam_CalledWithNullExpression_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => _navigator.WithParam(
                null,
                FluentNavigatorTestsDataProvider.NavigationParamValue));
        }

        [Fact]
        public void WithParam_CalledWithNotNullExpression_CreatesAndAddsParameter()
        {
            var initialParametersCount = _navigator.Parameters.Count;
            _navigator.WithParam(
                (vm) => vm.ObjectParameter,
                FluentNavigatorTestsDataProvider.NavigationParamValue);

            Assert.Equal(initialParametersCount + 1, _navigator.Parameters.Count);
            var lastAddedParam = _navigator.Parameters.Last();

            Assert.Same(
                FluentNavigatorTestsDataProvider.NavigationParamValue,
                lastAddedParam.Value);
            Assert.NotNull(lastAddedParam.PropertyInfo);
            Assert.Equal(
               nameof(ViewModelStub.ObjectParameter),
               lastAddedParam.PropertyInfo.PropertyName);
        }
    }
}
