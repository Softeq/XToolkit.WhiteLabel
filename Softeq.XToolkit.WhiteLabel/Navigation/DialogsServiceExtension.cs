// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class DialogsServiceExtension
    {
        public static DialogNavigationHelper<T> For<T>(this IDialogsService dialogsService) 
            where T : class, IDialogViewModel
        {
            return new DialogNavigationHelper<T>(dialogsService);
        }
    }
}
