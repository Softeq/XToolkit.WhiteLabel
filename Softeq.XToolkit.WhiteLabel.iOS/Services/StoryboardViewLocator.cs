using System;
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
        private readonly IAppDelegate _appDelegate;

        public StoryboardViewLocator(ILogManager logManager, IAppDelegate appDelegate)
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

        public UIViewController GetView(object model)
        {
            var controller = GetViewFromPredefinedTypes(model) ?? GetControllerForModel(model.GetType());
            controller.SetExistingViewModel(model);
            return controller;
        }

        private ViewControllerBase GetViewFromPredefinedTypes(object model)
        {
            if (_modelToControllerTypes.TryGetValue(model.GetType(), out var controllerType))
            {
                var storyboardName = controllerType.Name.Replace("ViewController", "Storyboard");

                return TryCreateViewController(storyboardName, controllerType.Name);
            }

            return null;
        }

        public UIViewController GetTopViewController()
        {
            return UIApplication.SharedApplication.Windows
                .Where(window => window.RootViewController != null)
                .Select(window => _appDelegate.GetRootViewFinder(window.RootViewController))
                .FirstOrDefault();
        }

        private ViewControllerBase GetControllerForModel(Type type)
        {
            var storyBoardName = type.Name.Replace("ViewModel", "Storyboard");
            var targetTypeName = type.Name.Replace("ViewModel", "ViewController");

            return TryCreateViewController(storyBoardName, targetTypeName) ?? CreateWithActivator(type.FullName);
        }

        private ViewControllerBase TryCreateViewController(string storyBoardName, string targetTypeName)
        {
            ViewControllerBase newViewController = null;

            try
            {
                Execute.OnUIThread(() =>
                {
                    var storyboard = UIStoryboard.FromName(storyBoardName, null);
                    newViewController = (ViewControllerBase)storyboard.InstantiateViewController(targetTypeName);
                });
            }
            catch (Exception ex)
            {
                _logger.Warn(ex);
            }

            return newViewController;
        }

        private static ViewControllerBase CreateWithActivator(string viewModelType)
        {
            var targetTypeName = viewModelType.Replace(".ViewModels.", ".iOS.ViewControllers.");
            targetTypeName = targetTypeName.Replace("ViewModel", "ViewController");

            var targetType = Type.GetType(targetTypeName)
                             ?? AssemblySource.FindTypeByNames(new[] { targetTypeName });

            if (targetType == null)
            {
                throw new DllNotFoundException($"Can't find target type: {targetTypeName}");
            }

            return (ViewControllerBase)Activator.CreateInstance(targetType);
        }
    }
}