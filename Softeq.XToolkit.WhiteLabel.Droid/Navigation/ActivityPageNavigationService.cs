// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Droid.Interfaces;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class ActivityPageNavigationService : IPlatformNavigationService
    {
        private readonly IBundleService _bundleService;
        private readonly IViewLocator _viewLocator;
        private readonly IContextProvider _contextProvider;

        public ActivityPageNavigationService(
            IBundleService bundleService,
            IViewLocator viewLocator,
            IContextProvider contextProvider)
        {
            _bundleService = bundleService;
            _viewLocator = viewLocator;
            _contextProvider = contextProvider;
        }

        public bool CanGoBack
        {
            get
            {
                var memberInfo = _contextProvider.CurrentActivity.GetType();
                return memberInfo.GetCustomAttribute(typeof(StartActivityAttribute)) == null;
            }
        }

        public void Initialize(object navigation)
        {
        }

        public void GoBack()
        {
            Execute.BeginOnUIThread(() =>
            {
                var activity = _contextProvider.CurrentActivity;

                if (CanGoBack)
                {
                    activity.Finish();
                }
                else
                {
                    activity.OnBackPressed();
                }
            });
        }

        public void NavigateToViewModel(
            IViewModelBase viewModelBase,
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
        {
            var type = _viewLocator.GetTargetType(viewModelBase.GetType(), ViewType.Activity);
            StartActivityImpl(type, clearBackStack, parameters);
        }

        private void StartActivityImpl(
            Type type,
            bool shouldClearBackStack = false,
            IReadOnlyList<NavigationParameterModel>? parameters = null)
        {
            var activity = _contextProvider.CurrentActivity;
            var intent = new Intent(activity, type);

            _bundleService.TryToSetParams(intent, parameters);

            if (shouldClearBackStack)
            {
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            }

            activity.StartActivity(intent);
        }
    }
}
