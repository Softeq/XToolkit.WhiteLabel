// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Linq;
using Android.Support.V4.App;
using Android.Support.V7.App;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;
using System;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class FrameNavigationService : IFrameNavigationService
    {
        private const string FrameNavigationServiceParameterName = "FrameNavigationService";

        private readonly IViewLocator _viewLocator;
        private readonly IIocContainer _iocContainer;
        private int _containerId;
        private readonly Stack<(IViewModelBase ViewModel, Fragment Fragment)> _backStack;

        public FrameNavigationService(IViewLocator viewLocator, IIocContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
            _backStack = new Stack<(IViewModelBase viewModelBase, Fragment fragment)>();
        }

        public bool CanGoBack => _backStack.Count > 1;

        public bool IsInitialized => _containerId != 0;

        public IViewModelBase CurrentViewModel => _backStack.Peek().ViewModel;

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

        internal Fragment GetTopFragment()
        {
            return _backStack.FirstOrDefault().Fragment;
        }

        //TODO: replace with For<>.WithParam implementation
        public void NavigateToViewModel<T, TParameter>(TParameter parameter)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            var viewModel = _iocContainer.Resolve<T>();
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);
            viewModel.Parameter = parameter;
            NavigateInternal(viewModel);
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            var viewModel = _iocContainer.Resolve<T>();
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);

            if (clearBackStack)
            {
                _backStack.Clear();
            }

            NavigateInternal(viewModel);
        }

        public void NavigateToViewModel(Type viewModelType, bool clearBackStack = false)
        {
            if(!viewModelType.GetInterfaces().Any(x => x.Equals(typeof(IViewModelBase))))
            {
                throw new Exception("Class must implement IViewModelBase");
            }

            var viewModel = (IViewModelBase) _iocContainer.Resolve(viewModelType);
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);

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
            var viewModel = _iocContainer.Resolve<TViewModel>();
            viewModel.ApplyParameters(navigationParameters);
            NavigateToViewModel(viewModel);
        }

        public void RestoreState()
        {
            ReplaceFragment(_backStack.Peek().Fragment);
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
            var activity = (AppCompatActivity) CrossCurrentActivity.Current.Activity;
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
    }
}