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

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class FrameNavigationService : IFrameNavigationService
    {
        private const string FrameNavigationServiceParameterName = "FrameNavigationService";

        private readonly Stack<ViewModelBase> _backStack;
        private readonly ViewLocator _viewLocator;
        private readonly IIocContainer _iocContainer;
        private int _containerId;

        public FrameNavigationService(ViewLocator viewLocator, IIocContainer iocContainer)
        {
            _viewLocator = viewLocator;
            _iocContainer = iocContainer;
            _backStack = new Stack<ViewModelBase>();
        }

        public bool CanGoBack => _backStack.Count > 1;

        public int BackStackCount => _backStack.Count;

        public bool IsInitialized => _containerId != 0;

        public ViewModelBase CurrentViewModel => _backStack.Peek();

        public void GoBack()
        {
            Execute.BeginOnUIThread(() =>
            {
                _backStack.Pop();
                RestoreState();
            });
        }

        public void GoBack<T>() where T : ViewModelBase
        {
            var viewModel = _backStack.FirstOrDefault(x => x is T);
            if (viewModel == null)
            {
                return;
            }

            while (viewModel != _backStack.Peek())
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
            var viewModel = _iocContainer.Resolve<T>();
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);
            viewModel.Parameter = parameter;
            var baseFragment = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);
            _backStack.Push(viewModel as ViewModelBase);
            ReplaceFragment(baseFragment);
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            var viewModel = _iocContainer.Resolve<T>();
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);
            var fragment = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);

            if (clearBackStack)
            {
                _backStack.Clear();
            }

            _backStack.Push(viewModel as ViewModelBase);
            ReplaceFragment(fragment);
        }

        public void NavigateToViewModel<T>(T t) where T : IViewModelBase
        {
            if (Contains(_backStack, t))
            {
                while (ReferenceEquals(t, _backStack.Peek()))
                {
                    _backStack.Pop();
                }

                Execute.BeginOnUIThread(RestoreState);
            }
            else
            {
                NavigateToExistingViewModel(t as ViewModelBase);
            }
        }

        public void RestoreState()
        {
            var viewModel = _backStack.Peek();
            var view = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);
            ReplaceFragment(view);
        }

        private void NavigateToExistingViewModel(ViewModelBase viewModel)
        {
            _viewLocator.TryInjectParameters(viewModel, this, FrameNavigationServiceParameterName);
            var baseFragment = (Fragment) _viewLocator.GetView(viewModel, ViewType.Fragment);
            _backStack.Push(viewModel);
            ReplaceFragment(baseFragment);
        }

        private void ReplaceFragment(Fragment fragment)
        {
            var activity = (AppCompatActivity) CrossCurrentActivity.Current.Activity;
            activity.SupportFragmentManager
                .BeginTransaction()
                .Replace(_containerId, fragment)
                .Commit();
        }

        private static bool Contains(Stack<ViewModelBase> stack, IViewModelBase viewModelBase)
        {
            foreach (var item in stack)
            {
                if (ReferenceEquals(item, viewModelBase))
                {
                    return true;
                }
            }

            return false;
        }
    }
}