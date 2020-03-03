// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using Softeq.XToolkit.Bindings.Abstract;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper;
using Softeq.XToolkit.WhiteLabel.iOS.Helpers;
using Softeq.XToolkit.WhiteLabel.iOS.Interfaces;
using Softeq.XToolkit.WhiteLabel.iOS.Navigation;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Services
{
    public class StoryboardViewLocator : IViewLocator
    {
        private readonly ILogger _logger;
        private readonly IViewControllerProvider _viewControllerProvider;
        private readonly ViewModelToViewMap _viewModelToViewMap;

        public StoryboardViewLocator(
            ILogManager logManager,
            IViewControllerProvider viewControllerProvider,
            ViewModelToViewMap viewModelToViewMap)
        {
            _viewControllerProvider = viewControllerProvider;
            _logger = logManager.GetLogger<StoryboardViewLocator>();
            _viewModelToViewMap = viewModelToViewMap;
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
            var viewControllerType = GetTargetViewType(viewModel.GetType());
            var storyboardName = BuildStoryboardName(viewControllerType);
            var viewController = ViewControllerHelper.TryCreateViewController(storyboardName, viewControllerType, _logger);

            if (viewController == null)
            {
                throw new InvalidOperationException($"Can't find VC for type: {viewControllerType}");
            }

            if (viewController is IBindable bindableViewController)
            {
                bindableViewController.SetDataContext(viewModel);
            }

            return viewController;
        }

        // TODO YP: export to base, looks the same as Android impl.
        protected virtual Type GetTargetViewType(Type viewModelType)
        {
            if (_viewModelToViewMap.TryGetValue(viewModelType, out var targetViewType))
            {
                return targetViewType;
            }

            var targetTypeName = BuildViewTypeName(viewModelType);
            targetViewType = Type.GetType(targetTypeName) ?? AssemblySource.FindTypeByNames(new[] { targetTypeName });

            if (targetViewType == null)
            {
                throw new InvalidOperationException($"Can't find target view type: {targetTypeName}");
            }

            return targetViewType;
        }

        protected virtual string BuildViewTypeName(Type viewType)
        {
            if (viewType == null)
            {
                throw new ArgumentNullException(nameof(viewType));
            }

            return viewType.FullName?
                .Replace(".ViewModels.", ".iOS.ViewControllers.")
                .Replace("ViewModel", "ViewController");
        }

        protected virtual string BuildStoryboardName(Type viewControllerType)
        {
            return viewControllerType.Name.Replace("ViewController", "Storyboard");
        }
    }
}
