// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Softeq.XToolkit.WhiteLabel.Droid.Providers;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class ActivityPageNavigationService : IPlatformNavigationService
    {
        private readonly IBundleService _bundleService;
        private readonly IViewLocator _viewLocator;

        public ActivityPageNavigationService(
            IBundleService bundleService,
            IViewLocator viewLocator)
        {
            _bundleService = bundleService;
            _viewLocator = viewLocator;
        }

        public bool CanGoBack
        {
            get
            {
                var memberInfo = CurrentActivity.GetType();
                return memberInfo.GetCustomAttribute(typeof(StartActivityAttribute)) == null;
            }
        }

        protected Android.App.Activity CurrentActivity
        {
            get
            {
                var currentActivity = Dependencies.Container.Resolve<IActivityProvider>().Current;
                return currentActivity;
            }
        }

        public void Initialize(object navigation)
        {
        }

        public void GoBack()
        {
            Execute.BeginOnUIThread(() =>
            {
                var activity = CurrentActivity;

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
            var activity = CurrentActivity;
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
