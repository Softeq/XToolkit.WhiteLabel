// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using UIKit;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class StoryboardViewLocator : IViewLocator
    {
        private const string FrameNavigationServiceParameterName = "FrameNavigationService";

        private readonly IIocContainer _iocContainer;
        private readonly Func<UIViewController, UIViewController> _getViewControllerFunc;
        private readonly ILogger _logger;

        public StoryboardViewLocator(
            ILogManager logManager,
            IIocContainer iocContainer,
            Func<UIViewController, UIViewController> getViewControllerFunc)
        {
            _iocContainer = iocContainer;
            _getViewControllerFunc = getViewControllerFunc;
            _logger = logManager.GetLogger<StoryboardViewLocator>();
        }

        public UIViewController GetView<T, TParameter>(TParameter parameter,
            IFrameNavigationService frameNavigationService = null)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            var viewModel = _iocContainer.Resolve<T>();
            var controller = GetView(viewModel);
            TryInjectParameters(viewModel, frameNavigationService, FrameNavigationServiceParameterName);
            TryInjectParameters(viewModel, parameter, "Parameter");
            return controller;
        }

        public UIViewController GetView<T>(IFrameNavigationService frameNavigationService = null)
            where T : IViewModelBase
        {
            var viewModel = _iocContainer.Resolve<T>();
            var controller = GetView(viewModel);
            TryInjectParameters(viewModel, frameNavigationService, FrameNavigationServiceParameterName);
            return controller;
        }

        public UIViewController GetView(object model)
        {
            var controller = GetControllerForModel(model.GetType());
            var method = controller.GetType().GetMethod("SetExistingViewModel");
            method?.Invoke(controller, new[] { model });
            return controller;
        }

        public UIViewController GetTopViewController()
        {
            if (_getViewControllerFunc == null)
            {
                throw new Exception("you should set _getViewControllerFunc from ctor");
            }

            for (var i = 0; i < UIApplication.SharedApplication.Windows.Length; i++)
            {
                var rootViewController = UIApplication.SharedApplication.Windows[i].RootViewController;
                if (rootViewController != null)
                {
                    return _getViewControllerFunc(rootViewController);
                }
            }

            return null;
        }

        private UIViewController GetControllerForModel(Type type)
        {
            var typeName = type.Name;

            var storyBoardName = typeName.Replace("ViewModel", "Storyboard");
            var targetTypeName = typeName.Replace("ViewModel", "ViewController");

            if (TryCreateViewControllerFromStoryboard(storyBoardName, targetTypeName, out var controller))
            {
                return controller;
            }

            return CreateWithActivator(type.FullName);
        }

        private bool TryCreateViewControllerFromStoryboard(string storyBoardName, string targetTypeName,
            out UIViewController viewController)
        {
            viewController = default(UIViewController);
            var result = false;
            try
            {
                var newViewController = default(UIViewController);
                Execute.OnUIThread(() =>
                {
                    var storyboard = UIStoryboard.FromName(storyBoardName, null);
                    newViewController = storyboard.InstantiateViewController(targetTypeName);
                });
                viewController = newViewController;
                result = true;
            }
            catch (Exception ex)
            {
                _logger.Warn(ex);
            }

            return result;
        }

        private static UIViewController CreateWithActivator(string viewModelType)
        {
            var targetTypeName = viewModelType.Replace(".ViewModels.", ".iOS.ViewControllers.");
            targetTypeName = targetTypeName.Replace("ViewModel", "ViewController");

            var targeType = Type.GetType(targetTypeName)
                            ?? AssemblySource.FindTypeByNames(new[] { targetTypeName });

            if (targeType == null)
            {
                throw new DllNotFoundException($"Can't find target type: {targetTypeName}");
            }

            return (UIViewController)Activator.CreateInstance(targeType);
        }

        #region inject parameter

        [Obsolete("Use another way for inject params (for example WithParam() navigation)")]
        private static void TryInjectParameters(object viewModel, object parameter, string parameterName)
        {
            var viewModelType = viewModel.GetType();

            var property = GetPropertyCaseInsensitive(viewModelType, parameterName);

            if (property == null)
            {
                return;
            }

            property.SetValue(viewModel, CoerceValue(property.PropertyType, parameter));
        }

        private static PropertyInfo GetPropertyCaseInsensitive(Type type, string propertyName)
        {
            var typeInfo = type.GetTypeInfo();
            var typeList = new List<Type> { type };

            if (typeInfo.IsInterface)
            {
                typeList.AddRange(typeInfo.ImplementedInterfaces);
            }

            return typeList
                .Select(interfaceType => interfaceType.GetRuntimeProperty(propertyName))
                .FirstOrDefault(property => property != null);
        }

        private static object CoerceValue(Type destinationType, object providedValue)
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

        private static object GetDefaultValue(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            return typeInfo.IsClass || typeInfo.IsInterface ? null : Activator.CreateInstance(type);
        }

        #endregion
    }
}