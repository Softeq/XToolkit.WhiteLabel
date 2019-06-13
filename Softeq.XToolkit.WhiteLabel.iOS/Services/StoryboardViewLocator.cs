// Developed by Softeq Development Corporation
// http://www.softeq.com

﻿using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.Common.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using UIKit;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class StoryboardViewLocator : IViewLocator
    {
        private Dictionary<Type, Type> _modelToControllerTypes;
        private readonly ILogger _logger;
        private readonly IViewControllerProvider _appDelegate;

        public StoryboardViewLocator(ILogManager logManager, IViewControllerProvider appDelegate)
        {
            _appDelegate = appDelegate;
            _logger = logManager.GetLogger<StoryboardViewLocator>();
            _modelToControllerTypes = new Dictionary<Type, Type>();
        }

        public void Initialize(Dictionary<Type, Type> viewModelToViewController)
        {
            if (viewModelToViewController == null)
            {
                return;
            }

            _modelToControllerTypes = viewModelToViewController;
        }

        public UIViewController GetTopViewController()
        {
            return UIApplication.SharedApplication.Windows
                .Where(window => window.RootViewController != null)
                .Select(window => _appDelegate.GetRootViewController(window.RootViewController))
                .FirstOrDefault();
        }

        public UIViewController GetView(object viewModel)
        {
            var controllerType = GetTargetType(viewModel.GetType());
            var storyboardName = controllerType.Name.Replace("ViewController", "Storyboard");
            var viewController = TryCreateViewController(storyboardName, controllerType);
            var method = viewController.GetType().GetMethod("SetExistingViewModel");
            method?.Invoke(viewController, new[] { viewModel });
            return viewController;
        }

        private Type GetTargetType(Type type)
        {
            Type targetType;

            if (_modelToControllerTypes.TryGetValue(type, out targetType))
            {
                return targetType;
            }

            var targetTypeName = type.FullName.Replace(".ViewModels.", ".iOS.ViewControllers.");
            targetTypeName = targetTypeName.Replace("ViewModel", "ViewController");
            targetType = Type.GetType(targetTypeName)
                ?? AssemblySource.FindTypeByNames(new[] { targetTypeName });
            if (targetType == null)
            {
                throw new Exception($"Can't find target type: {targetTypeName}");
            }

            return targetType;
        }

        private UIViewController TryCreateViewController(string storyBoardName, Type targetType)
        {
            UIViewController newViewController = null;

            try
            {
                Execute.OnUIThread(() =>
                {
                    if (Foundation.NSBundle.MainBundle.PathForResource(storyBoardName, "storyboardc") != null)
                    {
                        var storyboard = UIStoryboard.FromName(storyBoardName, null);
                        newViewController = storyboard.InstantiateViewController(targetType.Name);
                    }
                    else
                    {
                        newViewController = (UIViewController) Activator.CreateInstance(targetType);
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.Warn(ex);
            }

            return newViewController;
        }
    }
}