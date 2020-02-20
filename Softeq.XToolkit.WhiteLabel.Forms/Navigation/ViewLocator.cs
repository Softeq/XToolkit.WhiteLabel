// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Xamarin.Forms;

namespace Softeq.XToolkit.WhiteLabel.Forms.Navigation
{
    public class ViewLocator : IViewLocator
    {
        public Page GetPage(object viewModel)
        {
            var type = viewModel.GetType();
            var targetTypeName = type.FullName.Replace(".ViewModels.", ".Views.");
            targetTypeName = targetTypeName.Replace("ViewModel", string.Empty);
            var targetType = Type.GetType(targetTypeName)
                             ?? AssemblySource.FindTypeByNames(new[] { targetTypeName });

            var page = (Page) Activator.CreateInstance(targetType);
            page.BindingContext = viewModel;
            return page;
        }
    }
}
