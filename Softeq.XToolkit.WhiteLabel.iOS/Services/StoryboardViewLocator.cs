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

        public UIViewController GetTopViewController()
        {
            return UIApplication.SharedApplication.Windows
                .Where(window => window.RootViewController != null)
                .Select(window => _appDelegate.GetRootViewFinder(window.RootViewController))
                .FirstOrDefault();
        }

        public UIViewController GetView(object viewModel)
        {
            var controllerType = GetTargetType();
            var storyboardName = controllerType.Name.Replace("ViewController", "Storyboard");
            var viewController = TryCreateViewController(storyboardName, controllerType);
            var method = viewController.GetType().GetMethod("SetExistingViewModel");
            method?.Invoke(viewController, new[] { viewModel });
            return viewController;
        }

        private static Type GetTargetType(Type type)
        {
            Type targetType;

            if (_modelToControllerTypes.TryGetValue(type, out targetType))
            {
                return targetType;
            }

            var targetTypeName = viewModelType.Replace(".ViewModels.", ".iOS.ViewControllers.");
            targetTypeName = targetTypeName.Replace("ViewModel", "ViewController");
            targetType = Type.GetType(targetTypeName)
                ?? AssemblySource.FindTypeByNames(new[] { targetTypeName });
            if (targetType == null)
            {
                throw new Exception($"Can't find target type: {targetTypeName}");
            }

            return targetType;
        }

        private ViewControllerBase TryCreateViewController(string storyBoardName, Type targetType)
        {
            ViewControllerBase newViewController = null;

            try
            {
                //TODO: VPY review this
                Execute.OnUIThread(() =>
                {
                    if (Foundation.NSBundle.MainBundle.PathForResource(storyBoardName, "storyboardc") != null)
                    {
                        var storyboard = UIStoryboard.FromName(storyBoardName, null);
                        newViewController = (ViewControllerBase)storyboard.InstantiateViewController(targetType.Name);
                    }
                    else
                    {
                        newViewController = (ViewControllerBase)Activator.CreateInstance(targetType);
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