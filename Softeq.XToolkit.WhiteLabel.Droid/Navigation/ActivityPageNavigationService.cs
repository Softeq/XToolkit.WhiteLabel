// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Reflection;
using Android.Content;
using Plugin.CurrentActivity;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class ActivityPageNavigationService : IPlatformNavigationService
    {
        private readonly IBundleService _bundleService;
        private readonly ICurrentActivity _currentActivity;
        private readonly IViewLocator _viewLocator;

        public ActivityPageNavigationService(
            IBundleService bundleService,
            ICurrentActivity currentActivity,
            IViewLocator viewLocator)
        {
            _bundleService = bundleService;
            _currentActivity = currentActivity;
            _viewLocator = viewLocator;
        }

        public void Initialize(object navigation)
        {
        }

        public bool CanGoBack
        {
            get
            {
                var memberInfo = _currentActivity.Activity.GetType();
                return memberInfo.GetCustomAttribute(typeof(StartActivityAttribute)) == null;
            }
        }

        public void GoBack()
        {
            Execute.BeginOnUIThread(() =>
            {
                var activity = _currentActivity.Activity;

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
            var activity = _currentActivity.Activity;
            var intent = new Intent(activity, type);

            _bundleService.TryToSetParams(intent, parameters);

            if (shouldClearBackStack)
            {
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            }

            activity.StartActivity(intent);
        }

        public void GoBack<TViewModel>() where TViewModel : IViewModelBase
        {
            throw new NotImplementedException();
        }

        public void GoToRoot()
        {
            throw new NotImplementedException();
        }
    }
}
