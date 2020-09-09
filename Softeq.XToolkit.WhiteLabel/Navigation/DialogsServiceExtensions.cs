// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class DialogsServiceExtensions
    {
        public static DialogFluentNavigator<T> For<T>(this IDialogsService dialogsService)
            where T : DialogViewModelBase
        {
            return new DialogFluentNavigator<T>(dialogsService);
        }
    }
}
