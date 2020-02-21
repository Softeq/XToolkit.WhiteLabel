// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Bootstrapper.Abstract;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsPageNavigationService : IPageNavigationService
    {
        private readonly IContainer _container;
        private readonly ILogger _logger;
        private readonly IFormsViewLocator _formsViewLocator;

        private INavigation? _navigation;

        public FormsPageNavigationService(
            ILogManager logManager,
            IFormsViewLocator formsViewLocator,
            IContainer container)
        {
            _formsViewLocator = formsViewLocator;
            _container = container;
            _logger = logManager.GetLogger<FormsPageNavigationService>();
        }

        private INavigation Navigation
        {
            get
            {
                if (_navigation == null)
                {
                    throw new Exception("Navigation service hasn't initialized");
                }

                return _navigation;
            }
        }

        public void Initialize(object navigation)
        {
            _navigation = (INavigation) navigation;
        }

        public bool CanGoBack => Navigation.NavigationStack.Count > 0;

        public void GoBack()
        {
            Navigation.PopAsync().FireAndForget(_logger);
        }

        public PageFluentNavigator<T> For<T>() where T : IViewModelBase
        {
            return new PageFluentNavigator<T>(this);
        }

        public void NavigateToViewModel<T>(bool clearBackStack, IReadOnlyList<NavigationParameterModel>? parameters)
            where T : IViewModelBase
        {
            var viewModel = _container.Resolve<T>();
            viewModel.ApplyParameters(parameters);

            var page = _formsViewLocator.GetPage(viewModel);

            if (clearBackStack)
            {
                Execute.BeginOnUIThread(() =>
                {
                    Navigation.InsertPageBefore(page, Navigation.NavigationStack.First());
                    Navigation.PopToRootAsync(false).FireAndForget(_logger);
                });
                return;
            }

            Execute.BeginOnUIThread(() => { Navigation.PushAsync(page).FireAndForget(_logger); });
        }
    }
}
