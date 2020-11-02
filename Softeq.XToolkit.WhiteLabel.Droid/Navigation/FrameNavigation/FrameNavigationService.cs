// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using AndroidX.AppCompat.App;
using AndroidX.Fragment.App;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    [Obsolete("Use DroidFrameNavigationService instead.")]
    public class FrameNavigationService : IFrameNavigationService
    {
        private readonly Stack<(IViewModelBase ViewModel, Fragment Fragment)> _backStack;
        private readonly IContainer _iocContainer;
        private readonly IViewLocator _viewLocator;
        private int _containerId;

        public FrameNavigationService(IViewLocator viewLocator, IContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
            _backStack = new Stack<(IViewModelBase ViewModel, Fragment Fragment)>();
        }

        public IViewModelBase CurrentViewModel => _backStack.Peek().ViewModel;

        public bool CanGoBack => _backStack.Count > 1;

        public bool IsInitialized => _containerId != 0;

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

            Execute.BeginOnUIThread(RestoreNavigation);
        }

        public void Initialize(object navigation)
        {
            _containerId = (int) navigation;
        }

        public void NavigateToFirstPage()
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

        public void RestoreNavigation()
        {
            ReplaceFragment(_backStack.Peek().Fragment);
        }

        public void NavigateToViewModel<TViewModel>(
            bool clearBackStack = false,
            // ReSharper disable once MethodOverloadWithOptionalParameter
            IReadOnlyList<NavigationParameterModel>? parameters = null) where TViewModel : IViewModelBase
        {
            throw new NotImplementedException();
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            var viewModel = _iocContainer.Resolve<T>(this);

            if (clearBackStack)
            {
                _backStack.Clear();
            }

            NavigateInternal(viewModel);
        }

        public void NavigateToViewModel<T>(T viewModel) where T : IViewModelBase
        {
            if (Contains(viewModel))
            {
                while (!ReferenceEquals(viewModel, _backStack.Peek().ViewModel))
                {
                    _backStack.Pop();
                }

                Execute.BeginOnUIThread(RestoreNavigation);
            }
            else
            {
                NavigateToExistingViewModel(viewModel);
            }
        }

        public void NavigateToViewModel<TViewModel>(IReadOnlyList<NavigationParameterModel> navigationParameters)
            where TViewModel : IViewModelBase
        {
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(navigationParameters);
            NavigateToViewModel(viewModel);
        }

        internal Fragment GetTopFragment()
        {
            return _backStack.FirstOrDefault().Fragment;
        }

        private void NavigateToExistingViewModel(IViewModelBase viewModel)
        {
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
            var currentActivity = _iocContainer.Resolve<IContextProvider>().CurrentActivity;
            var activity = (AppCompatActivity) currentActivity;
            var manager = activity.SupportFragmentManager;

            manager.BeginTransaction()
                .Replace(_containerId, fragment)
                .AddToBackStack(null)
                .Commit();
        }

        private bool Contains(IViewModelBase viewModelBase)
        {
            return _backStack.Any(item => ReferenceEquals(item.ViewModel, viewModelBase));
        }

        public void RestoreUnfinishedNavigation()
        {
        }
    }
}
