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
        public async Task<Page> GetPageAsync(object viewModel)
        {
            var type = viewModel.GetType();

            var viewTypeName = BuildViewTypeName(type.FullName);

            var targetType = Type.GetType(viewTypeName)
                             ?? AssemblySource.FindTypeByNames(new[] { viewTypeName });

            var page = (Page) Activator.CreateInstance(targetType);
            page.BindingContext = viewModel;

            if (viewModel is RootFrameNavigationViewModelBase rootFrameNavigationViewModelBase)
            {
                var navigationPage = (NavigationPage) page;
                await navigationPage.PushAsync(new Page(), false);
                rootFrameNavigationViewModelBase.FrameNavigationService.Initialize(page.Navigation);
                rootFrameNavigationViewModelBase.NavigateToFirstPage();
            }

            if (viewModel is IMasterDetailViewModel masterDetailViewModel)
            {
                var masterDetailPage = (MasterDetailPage) page;
                var masterPage = await GetPageAsync(masterDetailViewModel.MasterViewModel);
                masterPage.Title = "Master Page";
                masterDetailPage.Master = masterPage;
                if (masterDetailPage.Detail == null)
                {
                    masterDetailPage.Detail = new Page();
                }
            }

            return page;
        }

        public INavigation? FindNavigationForViewModel(INavigation navigation, object viewModel)
        {
            foreach (var page in navigation.NavigationStack)
            {
                if (viewModel == page.BindingContext)
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

        protected virtual string BuildViewTypeName(string viewModelTypeName)
        {
            var name = viewModelTypeName.Replace(".ViewModels.", ".Forms.Views.");
            name = name.Replace("ViewModel", string.Empty);
            return name;
        }
    }
}
