// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Softeq.XToolkit.Common.Threading;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsNavigationService : IPlatformNavigationService
    {
        private readonly IFormsViewLocator _formsViewLocator;

        private INavigation? _navigation;

        public FormsNavigationService(
            IFormsViewLocator formsViewLocator)
        {
            _formsViewLocator = formsViewLocator;
        }

        private INavigation Navigation
        {
            get
            {
                if (_navigation == null)
                {
                    throw new InvalidOperationException($"{nameof(FormsNavigationService)} hasn't initialized");
                }

                return _navigation;
            }
        }

        public bool CanGoBack => Navigation.NavigationStack.Count != 0;

        public async Task NavigateToViewModelAsync(
            IViewModelBase viewModelBase,
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
        {
            await NavigateAsync(viewModelBase, clearBackStack, parameters);
        }

        public void Initialize(object initParameter)
        {
            switch (initParameter)
            {
                case INavigation navigation:
                    _navigation = navigation;
                    return;
                case IViewModelBase viewModelBase:
                    var mainNavigation = Application.Current.MainPage.Navigation;
                    _navigation = _formsViewLocator.FindNavigationForViewModel(mainNavigation, viewModelBase);
                    return;
                default:
                    throw new InvalidOperationException($"{nameof(FormsNavigationService)} was not initialized");
            }
        }

        public async Task GoBackAsync()
        {
            await Navigation.PopAsync();
        }

        private async Task NavigateAsync(
            IViewModelBase viewModelBase,
            bool clearBackStack,
            IReadOnlyList<NavigationParameterModel>? parameters)
        {
            if (parameters != null)
            {
                viewModelBase.ApplyParameters(parameters);
            }

            var targetPage = await _formsViewLocator.GetPageAsync(viewModelBase);

            await Execute.OnUIThreadAsync(async () =>
            {
                if (clearBackStack && Navigation.NavigationStack.Count > 0)
                {
                    var currentPage = Navigation.NavigationStack.First();
                    Navigation.InsertPageBefore(targetPage, currentPage);

                    await Navigation.PopToRootAsync(false);
                }
                else
                {
                    await Navigation.PushAsync(targetPage);
                }
            });
        }
    }
}
