// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Android.Content;
using Android.OS;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public interface IBundleService
    {
        void SaveInstanceState(Bundle bundle);

        void TryToSetParams(Intent intent, IReadOnlyList<NavigationParameterModel>? parameters);

        /// <summary>
        ///
        ///     Skip restore when:
        ///     1) ViewModel was alive
        ///     2) Activity never been destroyed
        ///     3) we don't have data to restore.
        /// </summary>
        /// <param name="viewModel">ViewModel.</param>
        /// <param name="intent">Android Intent.</param>
        /// <param name="bundle">Android Bundle.</param>
        void TryToRestoreParams(ViewModelBase viewModel, Intent? intent, Bundle? bundle);
    }

    public class BundleService : IBundleService
    {
        private const string ShouldRestoreStateKey = "WL_ShouldRestore";
        private const string ParametersKey = "WL_Parameters";

        private readonly INavigationSerializer _serializer;

        public BundleService(INavigationSerializer serializer)
        {
            _serializer = serializer;
        }

        public void SaveInstanceState(Bundle bundle)
        {
            bundle.PutBoolean(ShouldRestoreStateKey, true);
        }

        public void TryToSetParams(Intent intent, IReadOnlyList<NavigationParameterModel>? parameters)
        {
            if (parameters != null && parameters.Any())
            {
                var serializedParameters = _serializer.Serialize(parameters);
                intent.PutExtra(ParametersKey, serializedParameters);
            }
        }

        /// <inheritdoc />
        public void TryToRestoreParams(ViewModelBase viewModel, Intent? intent, Bundle? bundle)
        {
            if (viewModel.IsInitialized)
            {
                return;
            }

            if (bundle == null || !bundle.ContainsKey(ShouldRestoreStateKey))
            {
                return;
            }

            if (intent == null || !intent.HasExtra(ParametersKey))
            {
                return;
            }

            var parametersObject = intent.GetStringExtra(ParametersKey);
            if (string.IsNullOrEmpty(parametersObject))
            {
                return;
            }

            var parameters = _serializer
                .Deserialize<IReadOnlyList<NavigationParameterModel>>(parametersObject)
                .EmptyIfNull();

            foreach (var parameter in parameters)
            {
                SetValueToProperty(viewModel, parameter);
            }

            intent.RemoveExtra(ShouldRestoreStateKey);
        }

        private void SetValueToProperty(ViewModelBase viewModel, NavigationParameterModel parameter)
        {
            var property = parameter.PropertyInfo?.ToPropertyInfo();

            if (property != null)
            {
                var value = DeserializeValueFromProperty(parameter.Value, property);

                property.SetValue(viewModel, value, null);
            }
        }

        private object? DeserializeValueFromProperty(object? value, PropertyInfo propertyInfo)
        {
            if (value == null)
            {
                return null;
            }

            if (propertyInfo.PropertyType.IsEnum)
            {
                return Enum.ToObject(propertyInfo.PropertyType, value);
            }

            return _serializer.Deserialize(value, propertyInfo.PropertyType);
        }
    }
}
