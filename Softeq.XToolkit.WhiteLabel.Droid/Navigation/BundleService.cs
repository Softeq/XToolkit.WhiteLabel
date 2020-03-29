// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Android.Content;
using Android.OS;
using Newtonsoft.Json.Linq;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public interface IBundleService
    {
        void TryToSetParams(Intent intent, IReadOnlyList<NavigationParameterModel>? parameters);

        /// <summary>
        ///
        ///     Skip restore when:
        ///     1) ViewModel was alive
        ///     2) Activity never been destroyed
        ///     3) we don't have data to restore
        /// </summary>
        /// <param name="viewModel"></param>
        /// <param name="intent"></param>
        /// <param name="bundle"></param>
        void TryToRestoreParams(ViewModelBase viewModel, Intent intent, Bundle bundle);

        void SaveInstanceState(Bundle bundle);
    }

    public class BundleService : IBundleService
    {
        private const string ShouldRestoreStateKey = "WL_ShouldRestore";
        private const string ParametersKey = "WL_Parameters";

        private readonly IJsonSerializer _jsonSerializer;

        public BundleService(
            IJsonSerializer jsonSerializer)
        {
            _jsonSerializer = jsonSerializer;
        }

        public void TryToSetParams(Intent intent, IReadOnlyList<NavigationParameterModel>? parameters)
        {
            if (parameters != null && parameters.Any())
            {
                intent.PutExtra(ParametersKey, _jsonSerializer.Serialize(parameters));
            }
        }

        /// <inheritdoc />
        public void TryToRestoreParams(ViewModelBase viewModel, Intent intent, Bundle bundle)
        {
            if (viewModel.IsInitialized
                || bundle == null
                || !bundle.ContainsKey(ShouldRestoreStateKey)
                || !intent.HasExtra(ParametersKey))
            {
                return;
            }

            var parametersObject = intent.GetStringExtra(ParametersKey);
            var parameters = _jsonSerializer
                .Deserialize<IReadOnlyList<NavigationParameterModel>>(parametersObject)
                .EmptyIfNull();

            foreach (var parameter in parameters)
            {
                SetValueToProperty(viewModel, parameter);
            }

            intent.RemoveExtra(ShouldRestoreStateKey);
        }

        public void SaveInstanceState(Bundle bundle)
        {
            bundle.PutBoolean(ShouldRestoreStateKey, true);
        }

        private static void SetValueToProperty(ViewModelBase viewModel, NavigationParameterModel parameter)
        {
            var property = parameter.PropertyInfo.ToProperty();

            object? GetValue(object? value)
            {
                if(value == null)
                {
                    return null;
                }

                if (property.PropertyType.IsEnum)
                {
                    return Enum.ToObject(property.PropertyType, value);
                }

                return ((JObject) value).ToObject(property.PropertyType);
            }

            property.SetValue(viewModel, GetValue(parameter.Value), null);
        }
    }
}
