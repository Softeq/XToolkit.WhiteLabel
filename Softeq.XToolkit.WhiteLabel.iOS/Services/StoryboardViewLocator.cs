// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class StoryboardViewLocator : IViewLocator
    {
        private readonly ILogger _logger;
        private readonly IViewControllerProvider _viewControllerProvider;

        private Dictionary<Type, Type> _modelToControllerTypes;

        public StoryboardViewLocator(
            ILogManager logManager,
            IViewControllerProvider viewControllerProvider)
        {
            _viewControllerProvider = viewControllerProvider;
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
                .Select(window => _viewControllerProvider.GetTopViewController(window.RootViewController))
                .FirstOrDefault();
        }

        public UIViewController GetView(object viewModel)
        {
            var controllerType = GetTargetType(viewModel.GetType());

            var storyboardName = controllerType.Name.Replace("ViewController", "Storyboard");
            var viewController = TryCreateViewController(storyboardName, controllerType);

            if (viewController == null)
            {
                throw new InvalidOperationException("view not found");
            }

            if (viewController is IBindable bindableViewController)
            {
                bindableViewController.SetDataContext(viewModel);
            }

            return viewController;
        }

        private Type GetTargetType(Type type)
        {
            if (_modelToControllerTypes.TryGetValue(type, out var targetType))
            {
                return targetType;
            }

            var targetTypeName = type.FullName.Replace(".ViewModels.", ".iOS.ViewControllers.");
            targetTypeName = targetTypeName.Replace("ViewModel", "ViewController");
            targetType = Type.GetType(targetTypeName) ?? AssemblySource.FindTypeByNames(new[] { targetTypeName });

            if (targetType == null)
            {
                throw new InvalidOperationException($"Can't find target type: {targetTypeName}");
            }

            return targetType;
        }

        // TODO YP: export to another class
        private UIViewController? TryCreateViewController(string storyBoardName, Type targetType)
        {
            UIViewController? newViewController = null;

            try
            {
                Execute.OnUIThread(() =>
                {
                    if (NSBundle.MainBundle.PathForResource(storyBoardName, "storyboardc") != null)
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
