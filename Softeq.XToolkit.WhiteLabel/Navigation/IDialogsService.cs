// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Model;
using Softeq.XToolkit.WhiteLabel.Mvvm;

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

        /// <summary>
        /// On Android this method can show up to three options
        /// and all of them must have different CommandActionStyle.
        /// They will be sorted in God-only-knows-what order: https://stackoverflow.com/questions/11401951/android-difference-between-positive-negative-and-neutral-button
        /// Probably this must be refactored to use radiobuttons.
        /// 
        /// On iOS there can be only one Cancel option (it will be in the bottom) and several others,
        /// which are not sorted.
        /// </summary>
        Task<TResult> ShowDialogAsync<TResult>(string title,
            string message,
            Dictionary<TResult, DialogOption> actions);

        [Obsolete("Please use ShowForViewModelAsync syntax instead")]
        Task ShowForViewModel<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel;

        [Obsolete("Please use ShowForViewModelAsync syntax instead")]
        Task<TResult> ShowForViewModel<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel;

        Task<IDialogResult> ShowForViewModelAsync<TViewModel>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel;

        Task<IDialogResult<TResult>> ShowForViewModelAsync<TViewModel, TResult>(
            IEnumerable<NavigationParameterModel> parameters = null)
            where TViewModel : IDialogViewModel;
    }
}
