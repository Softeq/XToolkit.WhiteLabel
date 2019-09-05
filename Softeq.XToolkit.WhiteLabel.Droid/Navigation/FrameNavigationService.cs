// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class FrameNavigationService : IFrameNavigationService
    {
        private const string FrameNavigationServiceParameterName = "FrameNavigationService";

        private readonly IViewLocator _viewLocator;
        private readonly ICurrentActivity _currentActivity;
        private readonly IContainer _container;
        private readonly Stack<(IViewModelBase ViewModel, Fragment Fragment)> _backStack;

        private int _containerId;

        public FrameNavigationService(
            IViewLocator viewLocator,
            ICurrentActivity currentActivity,
            IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _currentActivity = currentActivity;
            _container = iocContainer;
            _backStack = new Stack<(IViewModelBase viewModelBase, Fragment fragment)>();
        }

        public bool CanGoBack => _backStack.Count > 1;

        public bool IsInitialized => _containerId != 0;

        public IViewModelBase CurrentViewModel => _backStack.Peek().ViewModel;

        public bool IsEmptyBackStack => _backStack.Count == 0;

        public void GoBack()
        {
            Execute.BeginOnUIThread(() =>
            {
                _backStack.Pop();

                ReplaceFragment(_backStack.Peek().Fragment);
            });
        }

        public void GoBack<T>() where T : IViewModelBase
        {
            var viewModel = _backStack.FirstOrDefault(x => x.ViewModel is T).ViewModel;
            if (viewModel == null)
            {
                return;
            }

            while (viewModel != _backStack.Peek().ViewModel)
            {
                _backStack.Pop();
            }

            Execute.BeginOnUIThread(RestoreState);
        }

        public void Initialize(object navigation)
        {
            _containerId = (int) navigation;
        }

        public void NavigateToViewModel<T, TParameter>(TParameter parameter)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            var viewModel = _container.Resolve<T>();
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);
            viewModel.Parameter = parameter;
            NavigateInternal(viewModel);
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            var viewModel = _container.Resolve<T>();
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);

            if (clearBackStack)
            {
                _backStack.Clear();
            }

            NavigateInternal(viewModel);
        }

        public void NavigateToViewModel(Type viewModelType, bool clearBackStack = false)
        {
            if (!typeof(IViewModelBase).IsAssignableFrom(viewModelType))
            {
                throw new ArgumentException("Class must implement IViewModelBase");
            }

            var viewModel = (IViewModelBase) _container.Resolve(viewModelType);
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);

            if (clearBackStack)
            {
                _backStack.Clear();
            }

            NavigateInternal(viewModel);
        }

        public void NavigatToFirstPage()
        {
            if (IsEmptyBackStack)
            {
                return;
            }

            while (_backStack.Count > 1)
            {
                _backStack.Pop();
            }

            var firstViewModel = _backStack.Pop();

            NavigateToExistingViewModel(firstViewModel.ViewModel);
        }

        public void NavigateToViewModel<T>(T viewModel) where T : IViewModelBase
        {
            if (Contains(viewModel))
            {
                while (!ReferenceEquals(viewModel, _backStack.Peek().ViewModel))
                {
                    _backStack.Pop();
                }

                Execute.BeginOnUIThread(RestoreState);
            }
            else
            {
                NavigateToExistingViewModel(viewModel);
            }
        }

        public void NavigateToViewModel<TViewModel>(IEnumerable<NavigationParameterModel> navigationParameters)
            where TViewModel : IViewModelBase
        {
            var viewModel = _container.Resolve<TViewModel>();
            viewModel.ApplyParameters(navigationParameters);
            NavigateToViewModel(viewModel);
        }

        public void RestoreState()
        {
            ReplaceFragment(_backStack.Peek().Fragment);
        }

        protected virtual FragmentTransaction PrepareTransaction(FragmentTransaction fragmentTransaction)
        {
            return fragmentTransaction;
        }

        private void NavigateToExistingViewModel(IViewModelBase viewModel)
        {
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);
            NavigateInternal(viewModel);
        }

        private void NavigateInternal(IViewModelBase viewModel)
        {
            var fragment = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);

            _backStack.Push((viewModel, fragment));
            ReplaceFragment(fragment);
        }

        private void ReplaceFragment(Fragment fragment)
        {
            var activity = (AppCompatActivity) _currentActivity.Activity;
            var manager = activity.SupportFragmentManager;

            var transaction = manager.BeginTransaction()
                .Replace(_containerId, fragment);

            PrepareTransaction(transaction).Commit();
        }

        private bool Contains(IViewModelBase viewModelBase)
        {
            return _backStack.Any(item => ReferenceEquals(item.ViewModel, viewModelBase));
        }
    }
}
