// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Extensions;
using Softeq.XToolkit.Common.Logger;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Softeq.XToolkit.WhiteLabel.Threading;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsNavigationService : IPlatformNavigationService
    {
        private readonly IFormsViewLocator _formsViewLocator;
        private readonly ILogger _logger;

        private INavigation? _navigation;

        public FormsNavigationService(
            IFormsViewLocator formsViewLocator,
            ILogManager logManager)
        {
            _formsViewLocator = formsViewLocator;
            _logger = logManager.GetLogger<FormsNavigationService>();
        }

        private INavigation Navigation
        {
            get
            {
                if (_navigation == null)
                {
                    throw new InvalidOperationException("Navigation service hasn't initialized");
                }

                return _navigation;
            }
        }

        public void NavigateToViewModel(
            IViewModelBase viewModelBase,
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
        {
            NavigateTask(viewModelBase, clearBackStack, parameters).FireAndForget(_logger);
        }

        public bool CanGoBack
        {
            get => Navigation.NavigationStack.Count != 0;
        }

        public void Initialize(object initParameter)
        {
            if (initParameter is INavigation navigation)
            {
                _navigation = navigation;
                return;
            }

            if (initParameter is IViewModelBase viewModelBase)
            {
                _navigation = _formsViewLocator.FindNavigationForViewModel(
                    Application.Current.MainPage.Navigation, viewModelBase);
            }
        }

        public void GoBack()
        {
            Navigation.PopAsync().FireAndForget(_logger);
        }

        private async Task NavigateTask(
            IViewModelBase viewModelBase,
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
        {
            viewModelBase.ApplyParameters(parameters);

            var page = await _formsViewLocator.GetPageAsync(viewModelBase);

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
