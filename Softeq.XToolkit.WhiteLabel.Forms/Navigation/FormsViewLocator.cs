// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class FormsViewLocator : IFormsViewLocator
    {
        public Page GetPage(object viewModel)
        {
            var type = viewModel.GetType();

            var viewTypeName = BuildViewTypeName(type.FullName);
            viewTypeName = viewTypeName.Replace("ViewModel", string.Empty);

            var targetType = Type.GetType(viewTypeName)
                             ?? AssemblySource.FindTypeByNames(new[] { viewTypeName });

            var page = (Page) Activator.CreateInstance(targetType);
            page.BindingContext = viewModel;

            if (viewModel is IMasterDetailViewModel masterDetailViewModel)
            {
                var masterDetailPage = (MasterDetailPage) page;
                var masterPage = GetPage(masterDetailViewModel.MasterViewModel);
                masterPage.Title = "Master Page";
                masterDetailPage.Master = masterPage;
                if (masterDetailPage.Detail == null)
                {
                    masterDetailPage.Detail = new Page();
                }
            }

            return page;
        }

        protected virtual string BuildViewTypeName(string viewModelTypeName)
        {
            return viewModelTypeName.Replace(".ViewModels.", ".Forms.Views.");
        }
    }
}
