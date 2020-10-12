// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Tests.Stubs;
using Xunit;

namespace Softeq.XToolkit.WhiteLabel.Tests.Navigation.FluentNavigators.DialogFluentNavigatorTests
{
    public class DialogFluentNavigatorTests
    {
        private readonly IDialogsService _dialogsService;
        private readonly DialogFluentNavigator<DialogViewModelStub> _navigator;

        public DialogFluentNavigatorTests()
        {
            _dialogsService = Substitute.For<IDialogsService>();
            _navigator = new DialogFluentNavigator<DialogViewModelStub>(_dialogsService);
        }

        [Fact]
        public void DialogFluentNavigator_IsFluentNavigatorBase()
        {
            Assert.IsAssignableFrom<FluentNavigatorBase<DialogViewModelStub>>(_navigator);
        }

        [Fact]
        public void Ctor_Null_ThrowsCorrectException()
        {
            Assert.Throws<ArgumentNullException>(() => new DialogFluentNavigator<DialogViewModelStub>(null));
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

        [Fact]
        public void WithAwaitResult_ReturnsSelf()
        {
            var newNavigator = _navigator.WithAwaitResult();

            Assert.Same(_navigator, newNavigator);
        }

        [Fact]
        public void Navigate_WithoutAwaitResult_OpensDialogWithSpecifiedParams()
        {
            _navigator.Navigate();

            _dialogsService.Received(1).ShowForViewModelAsync<DialogViewModelStub>(
                Arg.Is(_navigator.Parameters));
        }

        [Fact]
        public void Navigate_WithAwaitResult_OpensDialogWithSpecifiedParams()
        {
            _navigator.WithAwaitResult().Navigate();

            _dialogsService.Received(1).ShowForViewModelAsync<DialogViewModelStub>(
                Arg.Is(_navigator.Parameters));
        }

        [Fact]
        public async void NavigateWithResult_WithoutAwaitResult_OpensDialogWithSpecifiedParams()
        {
            await _navigator.Navigate<string>();

            await _dialogsService.Received(1).ShowForViewModelAsync<DialogViewModelStub, string>(
                Arg.Is(_navigator.Parameters));
        }

        [Fact]
        public async void NavigateWithResult_WithAwaitResult_OpensDialogWithSpecifiedParams()
        {
            await _navigator.WithAwaitResult().Navigate<string>();

            await _dialogsService.Received(1).ShowForViewModelAsync<DialogViewModelStub, string>(
                Arg.Is(_navigator.Parameters));
        }
    }
}
