// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsViewLocator : IFormsViewLocator
    {
        public INavigation? FindNavigationForViewModel(INavigation navigation, object viewModel)
        {
            foreach (var page in navigation.NavigationStack)
            {
                if (page.BindingContext == viewModel)
                {
                    return page.Navigation;
                }

                if (page is MasterDetailPage masterDetailPage)
                {
                    return FindNavigationForViewModel(masterDetailPage.Detail.Navigation, viewModel);
                }
            }

            return null;
        }

        public async Task<Page> GetPageAsync(object viewModel)
        {
            var page = CreatePage(viewModel);
            await SetupPage(page, viewModel);
            return page;
        }

        protected virtual Page CreatePage(object viewModel)
        {
            var viewModelType = viewModel.GetType();

            var pageTypeName = viewModel is RootFrameNavigationViewModelBase
                ? BuildRootFrameNavigationPageTypeName(viewModelType.FullName)
                : BuildPageTypeName(viewModelType.FullName);

            var pageType = Type.GetType(pageTypeName) ?? AssemblySource.FindTypeByNames(new[] { pageTypeName });
            return (Page) Activator.CreateInstance(pageType);
        }

        protected virtual async Task SetupPage(Page page, object viewModel)
        {
            page.BindingContext = viewModel;

            switch (viewModel)
            {
                case RootFrameNavigationViewModelBase rootFrameNavigationViewModelBase:
                    await SetupFrameNavigationPage((NavigationPage) page, rootFrameNavigationViewModelBase);
                    break;
                case IMasterDetailViewModel masterDetailViewModel:
                    await SetupMasterDetailsPage((MasterDetailPage) page, masterDetailViewModel);
                    break;
            }
        }

        protected virtual string BuildRootFrameNavigationPageTypeName(string viewModelTypeName)
        {
            var name = viewModelTypeName
                .Replace(".Mvvm.", ".Forms.Navigation.");
            name = name.Remove(name.IndexOf("ViewModel"));
            return name;
        }

        protected virtual string BuildPageTypeName(string viewModelTypeName)
        {
            var name = viewModelTypeName
                .Replace(".ViewModels.", ".Forms.Views.")
                .Replace("ViewModel", string.Empty);
            return name;
        }

        protected virtual async Task SetupFrameNavigationPage(
            NavigationPage navigationPage,
            RootFrameNavigationViewModelBase rootFrameNavigationViewModelBase)
        {
            await navigationPage.PushAsync(new Page(), false);
            rootFrameNavigationViewModelBase.InitializeNavigation(navigationPage.Navigation);
            rootFrameNavigationViewModelBase.NavigateToFirstPage();
        }

        protected virtual async Task SetupMasterDetailsPage(
            MasterDetailPage masterDetailsPage,
            IMasterDetailViewModel masterDetailsViewModel)
        {
            var masterPage = await GetPageAsync(masterDetailsViewModel.MasterViewModel);
            masterPage.Title = "Master Page";
            masterDetailsPage.Master = masterPage;
            if (masterDetailsPage.Detail == null)
            {
                masterDetailsPage.Detail = new Page();
            }
        }
    }
}
