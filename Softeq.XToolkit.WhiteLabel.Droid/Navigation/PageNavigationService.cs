// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class PageNavigationService : IPageNavigationService
    {
        private readonly Stack<IViewModelBase> _backStack;
        private readonly ViewLocator _viewLocator;
        private readonly IJsonSerializer _jsonSerializer;

        public PageNavigationService(ViewLocator viewLocator, IJsonSerializer jsonSerializer)
        {
            _viewLocator = viewLocator;
            _jsonSerializer = jsonSerializer;
            _backStack = new Stack<IViewModelBase>();
        }

        public bool CanGoBack => !CrossCurrentActivity.Current.Activity.IsTaskRoot;

        public IViewModelBase GetExistingOrCreateViewModel(Type type)
        {
            if (_backStack.TryPeek(out var viewModel))
            {
                return viewModel;
            }

            //Used to recreate viewmodel if processes or activity was killed
            viewModel = (IViewModelBase)ServiceLocator.Resolve(type);
            _backStack.Push(viewModel);

            return viewModel;
        }

        public void GoBack()
        {
            if (_backStack.Count != 0)
            {
                _backStack.Pop();
                CrossCurrentActivity.Current.Activity.Finish();
            }
            else
            {
                CrossCurrentActivity.Current.Activity.OnBackPressed();
            }
        }

        public void Initialize(object navigation)
        {
            //not used in this platform
        }

        public void NavigateToViewModel<T, TParameter>(TParameter parameter, bool clearBackStack = false)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            For<T>().WithParam(x => x.Parameter, parameter).Navigate(clearBackStack);
        }

        public NavigateHelper<T> For<T>() where T : IViewModelBase
        {
            return new NavigateHelper<T>(NavigateToViewModelInternal<T>);
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            NavigateToViewModelInternal<T>(clearBackStack);
        }

        public void RestoreState()
        {
            var viewModelType = _backStack.Peek();
            NavigateToExistingViewModel(viewModelType.GetType());
        }

        private void NavigateToExistingViewModel(Type viewModelType)
        {
            var type = _viewLocator.GetTargetType(viewModelType, ViewType.Activity);

            StartActivityImpl(type);
        }

        private void NavigateToViewModelInternal<T>(bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel> parameters = null) where T : IViewModelBase
        {
            if (clearBackStack)
            {
                _backStack.Clear();
            }

            var viewModel = ServiceLocator.Resolve<T>();
            NavigateHelper<T>.ApplyParametersToViewModel(viewModel, parameters);

            var type = _viewLocator.GetTargetType(typeof(T), ViewType.Activity);
            StartActivityImpl(type, clearBackStack, parameters);

            _backStack.Push(viewModel);
        }

        private void StartActivityImpl(Type type, bool shouldClearBackStack = false,
            IReadOnlyList<NavigationParameterModel> parameters = null)
        {
            var intent = new Intent(CrossCurrentActivity.Current.Activity, type);
            SetParameters(intent, parameters);

            if (shouldClearBackStack)
            {
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                _backStack.Clear();
            }

            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }

        private void SetParameters(Intent intent, IReadOnlyList<NavigationParameterModel> parameters)
        {
            if (parameters != null && parameters.Any())
            {
                intent.PutExtra(Constants.ParametersKey, _jsonSerializer.Serialize(parameters));
            }
        }
    }
}