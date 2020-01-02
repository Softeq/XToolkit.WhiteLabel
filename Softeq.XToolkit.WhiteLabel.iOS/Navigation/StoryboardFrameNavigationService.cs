// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Threading;

namespace Softeq.XToolkit.WhiteLabel.iOS.Navigation
{
    public class StoryboardFrameNavigationService : StoryboardNavigation, IFrameNavigationService
    {
        private readonly IContainer _iocContainer;

        public StoryboardFrameNavigationService(
            IViewLocator viewLocator,
            IContainer iocContainer)
            : base(viewLocator)
        {
            _iocContainer = iocContainer;
        }

        public bool IsEmptyBackStack => !NavigationController!.ViewControllers.Any();

        bool IFrameNavigationService.IsInitialized => NavigationController != null;

        bool IFrameNavigationService.CanGoBack => CanGoBack;

        public virtual void NavigateToViewModel<TViewModel>(
            bool clearBackStack = false,
            IReadOnlyList<NavigationParameterModel>? parameters = null)
            where TViewModel : IViewModelBase
        {
            var viewModel = CreateViewModel<TViewModel>();

            NavigateToViewModel(viewModel, clearBackStack, parameters);
        }

        void IFrameNavigationService.GoBack()
        {
            GoBack();
        }

        void IFrameNavigationService.GoBack<T>()
        {
            Execute.BeginOnUIThread(() =>
            {
                var controller = NavigationController!
                    .ChildViewControllers
                    .FirstOrDefault(x => x is ViewControllerBase<T>);

                if (controller != null)
                {
                    NavigationController.PopToViewController(controller, false);
                }
            });
        }

        void IFrameNavigationService.Initialize(object navigation)
        {
            Initialize(navigation);
        }

        void IFrameNavigationService.RestoreNavigation()
        {
            throw new InvalidOperationException();
        }

        void IFrameNavigationService.NavigateToFirstPage()
        {
            throw new InvalidOperationException();
        }

        protected virtual IViewModelBase CreateViewModel<TViewModel>()
        {
            if (!typeof(IViewModelBase).IsAssignableFrom(typeof(TViewModel)))
            {
                throw new ArgumentException($"Class must implement {nameof(IViewModelBase)}");
            }

            return (IViewModelBase) _iocContainer.Resolve<TViewModel>(this);
        }
    }
}