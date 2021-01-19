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

            Console.WriteLine(viewModelType.FullName);

            //var pageTypeName = string.Empty;

            var pageTypeName = viewModel is RootFrameNavigationViewModelBase
                ? BuildRootFrameNavigationPageTypeName(viewModelType)
                : BuildPageTypeName(viewModelType.FullName);

            //switch (viewModel)
            //{
            //    case RootFrameNavigationViewModelBase rootFrame:
            //        pageTypeName = BuildRootFrameNavigationPageTypeName(viewModelType.FullName);
            //        break;

            //    case TabViewModel tab:
            //        pageTypeName = BuildTab(viewModelType);
            //        break;

            //    default:
            //        pageTypeName = BuildPageTypeName(viewModelType.FullName);
            //}

            Console.WriteLine(pageTypeName);

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

        protected virtual string BuildRootFrameNavigationPageTypeName(Type? viewModelType)
        {
            Console.WriteLine("virtual BuildRootFrameNavigationPageTypeName");

            var name = viewModelType?.FullName ?? string.Empty;

            if (name.Contains(".Tab."))
            {
                //name.Replace(".Tab.", ".Forms.Navigation.");
                name = "Softeq.XToolkit.WhiteLabel.Forms.Navigation.RootFrameNavigationPage";
            }
            else
            {
                name = name.Replace(".Mvvm.", ".Forms.Navigation.").Remove(name.IndexOf("ViewModel"));
            }

            return name;
        }

        protected virtual string BuildPageTypeName(string viewModelTypeName)
        {
            Console.WriteLine("virtual BuildPageTypeName");

            //var name = viewModelTypeName
            //    .Replace(".ViewModels.", ".Forms.Views.")
            //    .Replace("ViewModel", string.Empty);
            //return name;

            var name = viewModelTypeName
                .Replace(".ViewModels.", ".Views.")
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

        protected virtual async Task SetupTabbedPage(
            TabbedPage tabbedPage,
            ToolbarViewModelBase<string> tabbedViewModel)
        {
            //foreach (var tabItem in tabbedViewModel.TabbedItems)
            //{
            //    var tabPage = await GetPageAsync(tabItem.ViewModel);
            //    tabPage.Title = tabItem.TabTitle;
            //    tabPage.IconImageSource = tabItem.TabSourceImage;

            //    tabbedPage.Children.Add(tabPage);
            //}

            tabbedViewModel.OnInitialize();

            foreach (var tabModel in tabbedViewModel.TabViewModels)
            {
                var tabPage = await GetPageAsync(tabModel);
                tabPage.Title = tabModel.Key;
                //tabPage.IconImageSource = tabModel.TabSourceImage;

                tabbedPage.Children.Add(tabPage);
            }
        }
    }
}
