// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation;
using Softeq.XToolkit.WhiteLabel.ViewModels.Tab;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsViewLocator : IFormsViewLocator
    {
        public const string RootFrameNavigationPagePath = "Softeq.XToolkit.WhiteLabel.Forms.Navigation.RootFrameNavigationPage";

        public INavigation? FindNavigationForViewModel(INavigation navigation, object viewModel)
        {
            foreach (var page in navigation.NavigationStack)
            {
                switch (page)
                {
                    case var p when p.BindingContext == viewModel:
                        return page.Navigation;
                    case MasterDetailPage masterDetailPage:
                        return FindNavigationForViewModel(masterDetailPage.Detail.Navigation, viewModel);
                    case TabbedPage tabbedPage:
                        return FindNavigationForViewModel(tabbedPage.CurrentPage.Navigation, viewModel);
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
                case ToolbarViewModelBase<string> tabbedViewModel:
                    await SetupTabbedPage((TabbedPage) page, tabbedViewModel);
                    break;
            }
        }

        protected virtual string BuildRootFrameNavigationPageTypeName(string viewModelTypeName)
        {
            //string? name;

            //if (viewModelTypeName.Contains(".Tab."))
            //{
            //    name = RootFrameNavigationPagePath;
            //}
            //else
            //{
            //    name = viewModelTypeName.Replace(".Mvvm.", ".Forms.Navigation.");
            //    name = name.Remove(name.IndexOf("ViewModel"));
            //}

            //return name;

            return RootFrameNavigationPagePath;
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
            //await navigationPage.PushAsync(new Page(), false);
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

        protected virtual async Task SetupTabbedPage(
            TabbedPage tabbedPage,
            ToolbarViewModelBase<string> tabbedViewModel)
        {
            tabbedViewModel.OnInitialize();

            foreach (var tabModel in tabbedViewModel.TabViewModels)
            {
                var tabPage = await GetPageAsync(tabModel);
                tabPage.Title = tabModel.Title;
                tabPage.IconImageSource = tabModel.Key;

                tabbedPage.Children.Add(tabPage);
            }
        }
    }
}
