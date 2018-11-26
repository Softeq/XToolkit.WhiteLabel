﻿// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using Softeq.XToolkit.Common;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;
using UIKit;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public abstract class StoryboardNavigation
    {
        protected readonly IViewLocator ViewLocator;

        private WeakReferenceEx<UINavigationController> _navigationControllerRef;

        protected StoryboardNavigation(IViewLocator viewLocator)
        {
            ViewLocator = viewLocator;
        }

        protected UINavigationController NavigationController
        {
            get => _navigationControllerRef?.Target;
            set => _navigationControllerRef = WeakReferenceEx.Create(value);
        }

        //protected int BackStackCount => NavigationController.ChildViewControllers.Length;

        public bool CanGoBack => NavigationController.ViewControllers.Length > 1;

        public void NavigateToViewModel<T, TParameter>(TParameter parameter, bool clearBackStack = false)
            where T : IViewModelBase, IViewModelParameter<TParameter>
        {
            For<T>().WithParam(x => x.Parameter, parameter).Navigate();
        }

        public void NavigateToViewModel<T>(bool clearBackStack = false) where T : IViewModelBase
        {
            NavigateToViewControllerInternal<T>(null, clearBackStack);
        }

        public NavigateHelper<T> For<T>() where T : IViewModelBase
        {
            return new NavigateHelper<T>((shouldClearBackStack, parameters) =>
            {
                NavigateToViewControllerInternal<T>(parameters, shouldClearBackStack);
            });
        }

        public IViewModelBase GetExistingOrCreateViewModel(Type type)
        {
            throw new NotImplementedException("Not supported in current platform");
        }

        public void Initialize(object navigation)
        {
            NavigationController = navigation as UINavigationController;
        }

        public void GoBack()
        {
            Execute.BeginOnUIThread(() => NavigationController.PopViewController(true));
        }

        private void NavigateToViewControllerInternal<T>(IReadOnlyList<NavigationParameterModel> parameters,
            bool clearBackStack) where T : IViewModelBase
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = ViewLocator.GetView<T>(this as IFrameNavigationService);

                if (controller is ViewControllerBase<T> viewControllerBase)
                {
                    NavigateHelper<T>.ApplyParametersToViewModel(viewControllerBase.ViewModel, parameters);
                }

                Navigate(controller, clearBackStack);
            });
        }

        protected void Navigate(UIViewController controller, bool clearBackStack)
        {
            if (clearBackStack)
            {
                NavigationController.SetViewControllers(new[] {controller}, false);
                return;
            }

            NavigationController.PushViewController(controller, true);
        }
    }
}