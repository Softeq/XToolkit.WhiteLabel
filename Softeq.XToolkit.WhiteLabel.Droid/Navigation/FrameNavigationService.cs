// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Internal;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class FrameNavigationService : IFrameNavigationService
    {
        private readonly IViewLocator _viewLocator;
        private readonly ICurrentActivity _currentActivity;
        private readonly IContainer _container;
        private readonly Stack<(IViewModelBase ViewModel, Fragment Fragment)> _backStack;

        private int _containerId;

        public FrameNavigationService(
            IViewLocator viewLocator,
            ICurrentActivity currentActivity,
            IContainer container)
        {
            _viewLocator = viewLocator;
            _currentActivity = currentActivity;
            _container = container;
            _backStack = new Stack<(IViewModelBase viewModelBase, Fragment fragment)>();
        }

        public bool IsInitialized => _containerId != 0;

        public bool IsEmptyBackStack => _backStack.Count == 0;

        public bool CanGoBack => _backStack.Count > 1;

        public void GoBack()
        {
            Execute.BeginOnUIThread(() =>
            {
                var fragment = _backStack.Peek().Fragment;

                _backStack.Pop();

                ViewModelCache.Remove(fragment);

                var fragmentManager = GetCurrentFragmentManager();

                if (fragmentManager.BackStackEntryCount > 0)
                {
                    fragmentManager.PopBackStack();
                }

                if (fragmentManager.BackStackEntryCount == 0)
                {
                    ClearBackStack();
                }

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

            if (viewModel is IFrameViewModel frameViewModel)
            {
                frameViewModel.FrameNavigationService = this;
            }

            viewModel.Parameter = parameter;
            NavigateInternal(viewModel);
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            NavigateToViewModel(typeof(T), clearBackStack);
        }

        public void NavigateToViewModel(Type viewModelType, bool clearBackStack = false)
        {
            if (!typeof(IViewModelBase).IsAssignableFrom(viewModelType))
            {
                throw new ArgumentException("Class must implement IViewModelBase");
            }

            var viewModel = (IViewModelBase) _container.Resolve(viewModelType);

            if (viewModel is IFrameViewModel frameViewModel)
            {
                frameViewModel.FrameNavigationService = this;
            }

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
            if (Contains(viewModel)) // TODO YP: pop fm backstack
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
            if (viewModel is IFrameViewModel frameViewModel)
            {
                frameViewModel.FrameNavigationService = this;
            }

            NavigateInternal(viewModel);
        }

        private void NavigateInternal(IViewModelBase viewModel)
        {
            var fragment = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);

            ViewModelCache.Add(fragment, viewModel);

            _backStack.Push((viewModel, fragment));

            ReplaceFragment(fragment);
        }

        private void ReplaceFragment(Fragment fragment)
        {
            var fragmentManager = GetCurrentFragmentManager();

            var transaction = fragmentManager
                .BeginTransaction()
                .Replace(_containerId, fragment)
                .AddToBackStack(null);

            PrepareTransaction(transaction).Commit();
        }

        private FragmentManager GetCurrentFragmentManager()
        {
            var activity = (AppCompatActivity) _currentActivity.Activity;
            return activity.SupportFragmentManager;
        }

        private bool Contains(IViewModelBase viewModelBase)
        {
            return _backStack.Any(item => ReferenceEquals(item.ViewModel, viewModelBase));
        }

        private void ClearBackStack()
        {
            while (_backStack.TryPop(out var entry))
            {
                ViewModelCache.Remove(entry.Fragment);
            }
        }
    }
}
