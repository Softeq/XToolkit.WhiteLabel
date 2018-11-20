// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Android.Content;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class PageNavigationService : IPageNavigationService
    {
        private readonly Stack<string> _backStack;
        private readonly ViewLocator _viewLocator;

        public PageNavigationService(ViewLocator viewLocator)
        {
            _viewLocator = viewLocator;
            _backStack = new Stack<string>();
        }

        public int BackStackCount => _backStack.Count;

        public bool CanGoBack => _backStack.Count > 1;

        public void GoBack()
        {
            _backStack.Pop();
            NavigateToExistingViewModel(_backStack.Peek());
        }

        public void Initialize(object navigation)
        {
            //not used in this platform
        }

        public void NavigateToViewModel<T, TParameter>(TParameter parameter, bool clearBackStack = false)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            var viewModel = ServiceLocator.Resolve<T>();
            viewModel.Parameter = parameter;
            NavigateToViewModel<T>(clearBackStack);
        }

        public NavigateHelper<T> For<T>() where T : IViewModelBase
        {
            return new NavigateHelper<T>(ServiceLocator.Resolve<T>(),
                shouldClearBackStack => NavigateToViewModelInternal(ServiceLocator.Resolve<T>(), shouldClearBackStack));
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            NavigateToViewModelInternal(ServiceLocator.Resolve<T>(), clearBackStack);
        }

        public void RestoreState()
        {
            var viewModelType = _backStack.Peek();
            NavigateToExistingViewModel(viewModelType);
        }

        private void NavigateToExistingViewModel(string viewModelType)
        {
            var type = _viewLocator.GetTargetType(viewModelType, ViewType.Activity);
            StartActivityImpl(type);
        }

        private void NavigateToViewModelInternal(IViewModelBase viewModel, bool clearBackStack = false)
        {
            if (clearBackStack)
            {
                _backStack.Clear();
            }

            _backStack.Push(viewModel.GetType().FullName);

            viewModel.OnNavigated();

            var type = _viewLocator.GetTargetType(viewModel.GetType(), ViewType.Activity);
            StartActivityImpl(type);
        }

        private void StartActivityImpl(Type type)
        {
            var intent = new Intent(CrossCurrentActivity.Current.Activity, type);
            var currentActivity = CrossCurrentActivity.Current.Activity;
            CrossCurrentActivity.Current.Activity.StartActivity(intent);
            currentActivity.Finish();
        }
    }
}