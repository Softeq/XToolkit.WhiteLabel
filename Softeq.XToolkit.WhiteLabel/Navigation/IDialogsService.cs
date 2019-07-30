// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Model;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IDialogsService
    {
        Task<bool> ShowDialogAsync(
            string title,
            string message,
            string okButtonText,
            string cancelButtonText = null,
            OpenDialogOptions options = null);

        Task<IDialogResult> ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel;

        Task<IDialogResult<TResult>> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel;
    }
}
