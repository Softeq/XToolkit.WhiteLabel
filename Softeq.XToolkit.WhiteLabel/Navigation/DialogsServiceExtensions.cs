// Developed by Softeq Development Corporation
// http://www.softeq.com

using Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public static class DialogsServiceExtensions
    {
        public static DialogFluentNavigator<T> For<T>(this IDialogsService dialogsService)
            where T : class, IDialogViewModel
        {
            return new DialogFluentNavigator<T>(dialogsService);
        }
    }
}
