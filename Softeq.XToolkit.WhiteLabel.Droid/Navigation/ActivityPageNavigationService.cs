// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Content;
using Plugin.CurrentActivity;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class ActivityPageNavigationService : IPlatformNavigationService
    {
        private readonly ICurrentActivity _currentActivity;
        private readonly IJsonSerializer _jsonSerializer;
        private readonly IViewLocator _viewLocator;

        private bool _isParamsSerializationEnabled;

        public ActivityPageNavigationService(
            IViewLocator viewLocator,
            IJsonSerializer jsonSerializer,
            ICurrentActivity currentActivity)
        {
            _viewLocator = viewLocator;
            _jsonSerializer = jsonSerializer;
            _currentActivity = currentActivity;

            _isParamsSerializationEnabled = true;
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
                if (CanGoBack)
                {
                    _currentActivity.Activity.Finish();
                }
                else
                {
                    _currentActivity.Activity.OnBackPressed();
                }
            });
        }

        public void NavigateToViewModel(IViewModelBase viewModelBase, bool clearBackStack,
            IReadOnlyList<NavigationParameterModel> parameters)
        {
            var type = _viewLocator.GetTargetType(viewModelBase.GetType(), ViewType.Activity);
            StartActivityImpl(type, clearBackStack, parameters);
        }

        public ActivityPageNavigationService DisableParameterSerialization()
        {
            _isParamsSerializationEnabled = false;

            return this;
        }

        private void StartActivityImpl(Type type, bool shouldClearBackStack = false,
            IReadOnlyList<NavigationParameterModel> parameters = null)
        {
            var intent = new Intent(CrossCurrentActivity.Current.Activity, type);
            TryToSetParameters(intent, parameters);

            if (shouldClearBackStack)
            {
                intent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            }

            _currentActivity.Activity.StartActivity(intent);
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
