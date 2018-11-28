﻿// Developed by Softeq Development Corporation
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
    public class PageNavigationService : IPageNavigationService, IInternalNavigationService
    {
        private readonly ViewLocator _viewLocator;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IBackStackManager _backStackManager;

        private bool _isParamsSerializationEnabled;

        public PageNavigationService(ViewLocator viewLocator, IJsonSerializer jsonSerializer,
            IBackStackManager backStackManager)
        {
            _viewLocator = viewLocator;
            _jsonSerializer = jsonSerializer;
            _backStackManager = backStackManager;

            _isParamsSerializationEnabled = true;
        }

        public bool CanGoBack => !CrossCurrentActivity.Current.Activity.IsTaskRoot;

        public void GoBack()
        {
            if (_backStackManager.Count != 0)
            {
                _backStackManager.PopViewModel();
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

        public PageNavigationService DisableParameterSerialization()
        {
            _isParamsSerializationEnabled = false;

            return this;
        }

        public void NavigateToViewModel<T, TParameter>(TParameter parameter, bool clearBackStack = false)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            For<T>().WithParam(x => x.Parameter, parameter).Navigate(clearBackStack);
        }

        public NavigateHelper<T> For<T>() where T : IViewModelBase
        {
            return new NavigateHelper<T>(this);
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            NavigateToViewModel<T>(clearBackStack, null);
        }

        public void NavigateToViewModel<T>(bool clearBackStack,
            IReadOnlyList<NavigationParameterModel> parameters) where T : IViewModelBase
        {
            if (clearBackStack)
            {
                _backStackManager.Clear();
            }

            var viewModel = ServiceLocator.Resolve<T>();
            viewModel.ApplyParameters(parameters);

            var type = _viewLocator.GetTargetType(viewModel.GetType(), ViewType.Activity);
            StartActivityImpl(type, clearBackStack, parameters);

            _backStackManager.PushViewModel(viewModel);
        }

        private void StartActivityImpl(Type type, bool shouldClearBackStack = false,
            IReadOnlyList<NavigationParameterModel> parameters = null)
        {
            var intent = new Intent(CrossCurrentActivity.Current.Activity, type);
            TryToSetParameters(intent, parameters);

            if (shouldClearBackStack)
            {
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
                _backStackManager.Clear();
            }

            CrossCurrentActivity.Current.Activity.StartActivity(intent);
        }

        private void TryToSetParameters(Intent intent, IReadOnlyList<NavigationParameterModel> parameters)
        {
            if (_isParamsSerializationEnabled && parameters != null && parameters.Any())
            {
                intent.PutExtra(Constants.ParametersKey, _jsonSerializer.Serialize(parameters));
            }
        }
    }
}