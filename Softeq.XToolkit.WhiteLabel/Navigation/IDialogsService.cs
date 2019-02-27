// Developed by Softeq Development Corporation
// http://www.softeq.com

using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Interfaces;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;
using Softeq.XToolkit.WhiteLabel.Navigation.NavigationHelpers;

namespace Softeq.XToolkit.WhiteLabel.Navigation
{
    public interface IDialogsService
    {
        OpenDialogOptions DefaultOptions { get; }

        Task<TViewModel> ShowForViewModel<TViewModel>(OpenDialogOptions options = null)
            where TViewModel : class, IDialogViewModel;

        Task<TViewModel> ShowForViewModel<TViewModel, TParameter>(TParameter parameter,
            OpenDialogOptions options = null)
            where TViewModel : class, IDialogViewModel, IViewModelParameter<TParameter>;

        Task<bool> ShowDialogAsync(string title,
            string message,
            string okButtonText,
            string cancelButtonText = null,
            OpenDialogOptions options = null);

        Task<IDialogViewModel> ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel;

        Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters)
            where TViewModel : IDialogViewModel where TResult : class;

        DialogNavigationHelper<T> For<T>() where T : IDialogViewModel;
    }
}