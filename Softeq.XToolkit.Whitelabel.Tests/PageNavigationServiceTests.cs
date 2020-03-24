// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using NSubstitute;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xunit;

namespace Softeq.XToolkit.Whitelabel.Tests
{
    public class PageNavigationServiceTests
    {
        private readonly IBackStackManager _backStackManager;
        private readonly IPlatformNavigationService _platformNavService;
        private readonly ViewModelStub _viewModelStub;
        private readonly PageNavigationService _pageNavigationService;

        public PageNavigationServiceTests()
        {
            _viewModelStub = new ViewModelStub();
            _platformNavService = Substitute.For<IPlatformNavigationService>();
            _backStackManager = Substitute.For<IBackStackManager>();

            var serviceLocator = Substitute.For<IContainer>();
            serviceLocator.Resolve<ViewModelStub>().Returns(_viewModelStub);

            _pageNavigationService =
                new PageNavigationService(_platformNavService, _backStackManager, serviceLocator);
        }

        [Fact]
        public void NavigateToViewModel_BackStackShouldCleared()
        {
            //Action
            _pageNavigationService.NavigateToViewModel<ViewModelStub>(true);

            //Assert
            _platformNavService.Received(1).NavigateToViewModel(Arg.Any<ViewModelBase>(), Arg.Any<bool>(),
                Arg.Any<IReadOnlyList<NavigationParameterModel>>());
            _backStackManager.Received(1).Clear();
            _backStackManager.Received(1).PushViewModel(_viewModelStub);
        }

        [Fact]
        public void NavigateToViewModel_BackStackShouldNotBeCleared()
        {
            //Action
            _pageNavigationService.NavigateToViewModel<ViewModelStub>();

            //Assert
            _platformNavService.Received(1).NavigateToViewModel(Arg.Any<ViewModelBase>(), Arg.Any<bool>(),
                Arg.Any<IReadOnlyList<NavigationParameterModel>>());
            _backStackManager.DidNotReceive().Clear();
            _backStackManager.Received(1).PushViewModel(_viewModelStub);
        }

        [Fact]
        public void InitializePageNavigationService_ShouldExecutePlatformInitialization()
        {
            //Action
            _pageNavigationService.Initialize(new object());

            //Assert
            _platformNavService.Received(1).Initialize(Arg.Any<object>());
        }

        [Fact]
        public void GoBack_ShouldNotExecutePopViewModelInBackStackManager()
        {
            _backStackManager.Count.Returns(0);

            //Action
            _pageNavigationService.GoBack();

            //Assert
            _backStackManager.DidNotReceive().PopViewModel();
            _platformNavService.Received(1).GoBack();
        }

        [Fact]
        public void GoBack_ShouldExecutePopViewModelInBackStackManager()
        {
            _backStackManager.Count.Returns(1);

            //Action
            _pageNavigationService.GoBack();

            //Assert
            _backStackManager.Received(1).PopViewModel();
            _platformNavService.Received(1).GoBack();
        }

        [Fact]
        public void
            NavigateThroughForMethodWithoutParameterAndClearingBackStack_ShouldHandleAsNavigateToViewModeMethod()
        {
            //Action
            _pageNavigationService.For<ViewModelStub>().Navigate();

            //Assert
            _platformNavService.Received(1).NavigateToViewModel(Arg.Any<ViewModelBase>(), Arg.Any<bool>(),
                Arg.Any<IReadOnlyList<NavigationParameterModel>>());
            _backStackManager.DidNotReceive().Clear();
            _backStackManager.Received(1).PushViewModel(_viewModelStub);
        }

        [Fact]
        public void
            NavigateThroughForMethodWithoutParameterWithClearingBackStack_ShouldHandleAsNavigateToViewModeMethod()
        {
            //Action
            _pageNavigationService.For<ViewModelStub>().Navigate(true);

            //Assert
            _platformNavService.Received(1).NavigateToViewModel(Arg.Any<ViewModelBase>(), Arg.Any<bool>(),
                Arg.Any<IReadOnlyList<NavigationParameterModel>>());
            _backStackManager.Received(1).Clear();
            _backStackManager.Received(1).PushViewModel(_viewModelStub);
        }

        [Fact]
        public void ExecuteForMethod_ShouldNotExecuteAnything()
        {
            //Action
            _pageNavigationService.For<ViewModelStub>();

            //Assert
            _platformNavService.DidNotReceive().NavigateToViewModel(Arg.Any<ViewModelBase>(), Arg.Any<bool>(),
                Arg.Any<IReadOnlyList<NavigationParameterModel>>());
            _backStackManager.DidNotReceive().Clear();
            _backStackManager.DidNotReceive().PushViewModel(_viewModelStub);
        }

        [Fact]
        public void NavigateWithForWithParameter_ShouldSetParametersProperly()
        {
            //Action
            _pageNavigationService.For<ViewModelStub>()
                .WithParam(x => x.IntParameter, 10)
                .WithParam(x => x.StringParameter, "test")
                .WithParam(x => x.ObjectParameter, new object())
                .Navigate();

            //Assert
            _platformNavService.Received(1).NavigateToViewModel(Arg.Any<ViewModelBase>(), Arg.Any<bool>(),
                Arg.Any<IReadOnlyList<NavigationParameterModel>>());
            Assert.Equal(10, _viewModelStub.IntParameter);
            Assert.Equal("test", _viewModelStub.StringParameter);
            Assert.True(_viewModelStub.ObjectParameter != null);
        }
    }
}