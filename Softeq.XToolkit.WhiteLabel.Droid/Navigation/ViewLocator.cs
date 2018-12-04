// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.WhiteLabel.Mvvm;

namespace Softeq.XToolkit.WhiteLabel.Droid.Navigation
{
    public class ViewLocator
    {
        public Type GetTargetType<T>(ViewType viewType)
        {
            return GetTargetType(typeof(T), viewType);
        }

        public Type GetTargetType(Type type, ViewType viewType)
        {
            var typeName = type.FullName;
            return GetTargetType(typeName, viewType);
        }

        public object GetView(ViewModelBase viewModel, ViewType viewType)
        {
            var targetType = GetTargetType(viewModel.GetType(), viewType);
            var inst = Activator.CreateInstance(targetType);
            var method = inst.GetType().GetMethod("SetExistingViewModel");
            method.Invoke(inst, new[] {viewModel});
            return inst;
        }

        public object GetView(IViewModelBase viewModel, ViewType viewType)
        {
            var targetType = GetTargetType(viewModel.GetType(), viewType);
            var inst = Activator.CreateInstance(targetType);
            var method = inst.GetType().GetMethod("SetExistingViewModel");
            method.Invoke(inst, new[] {viewModel});
            return inst;
        }

        public Type GetTargetType(string viewModelTypeName, ViewType viewType)
        {
            var targetTypeName = viewModelTypeName.Replace(".ViewModels.", ".Droid.Views.");
            targetTypeName = targetTypeName.Replace("ViewModel", viewType.ToString());

            var targeType = Type.GetType(targetTypeName)
                            ?? AssemblySource.FindTypeByNames(new[] {targetTypeName});

            if (targeType == null)
            {
                throw new DllNotFoundException("can't find target type");
            }

            return targeType;
        }

        #region inject parameter

        public void TryInjectParameters(object viewModel, object parameter, string parameterName)
        {
            var viewModelType = viewModel.GetType();

            var property = GetPropertyCaseInsensitive(viewModelType, parameterName);

            if (property == null)
            {
                return;
            }

            property.SetValue(viewModel, CoerceValue(property.PropertyType, parameter));
        }

        private PropertyInfo GetPropertyCaseInsensitive(Type type, string propertyName)
        {
            var typeInfo = type.GetTypeInfo();
            var typeList = new List<Type> {type};

            if (typeInfo.IsInterface)
            {
                typeList.AddRange(typeInfo.ImplementedInterfaces);
            }

            return typeList
                .Select(interfaceType => interfaceType.GetRuntimeProperty(propertyName))
                .FirstOrDefault(property => property != null);
        }

        private object CoerceValue(Type destinationType, object providedValue)
        {
            if (providedValue == null)
            {
                return GetDefaultValue(destinationType);
            }

            var providedType = providedValue.GetType();
            if (destinationType.IsAssignableFrom(providedType))
            {
                return providedValue;
            }

            try
            {
                if (destinationType.GetTypeInfo().IsEnum)
                {
                    if (providedValue is string stringValue)
                    {
                        return Enum.Parse(destinationType, stringValue, true);
                    }

                    return Enum.ToObject(destinationType, providedValue);
                }

                if (typeof(Guid).IsAssignableFrom(destinationType))
                {
                    if (providedValue is string stringValue)
                    {
                        return new Guid(stringValue);
                    }
                }
            }
            catch
            {
                return GetDefaultValue(destinationType);
            }

            try
            {
                return Convert.ChangeType(providedValue, destinationType, CultureInfo.CurrentCulture);
            }
            catch
            {
                return GetDefaultValue(destinationType);
            }
        }

        private object GetDefaultValue(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass || typeInfo.IsInterface ? null : Activator.CreateInstance(type);
        }

        #endregion
    }
}