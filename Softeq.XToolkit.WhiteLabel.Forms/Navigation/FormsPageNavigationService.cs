// Developed by Softeq Development Corporation
// http://www.softeq.com

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

        private INavigation _navigation;

        public FormsPageNavigationService(
            ILogManager logManager,
            IFormsViewLocator formsViewLocator,
            IContainer container)
        {
            _formsViewLocator = formsViewLocator;
            _container = container;
            _logger = logManager.GetLogger<FormsPageNavigationService>();
        }

        public void Initialize(object navigation)
        {
            _navigation = (INavigation) navigation;
        }

        public bool CanGoBack => _navigation.NavigationStack.Count > 0;

        public void GoBack()
        {
            _navigation.PopAsync().FireAndForget(_logger);
        }

        public PageFluentNavigator<T> For<T>() where T : IViewModelBase
        {
            return new PageFluentNavigator<T>(this);
        }

        public void NavigateToViewModel<T>(bool clearBackStack, IReadOnlyList<NavigationParameterModel> parameters)
            where T : IViewModelBase
        {
            var viewModel = _container.Resolve<T>();
            viewModel.ApplyParameters(parameters);

            var page = _formsViewLocator.GetPage(viewModel);

            if (clearBackStack)
            {
                Execute.BeginOnUIThread(() =>
                {
                    _navigation.InsertPageBefore(page, _navigation.NavigationStack.First());
                    _navigation.PopToRootAsync(false).FireAndForget(_logger);
                });
                return;
            }

            Execute.BeginOnUIThread(() => { _navigation.PushAsync(page).FireAndForget(_logger); });
        }
    }
}
