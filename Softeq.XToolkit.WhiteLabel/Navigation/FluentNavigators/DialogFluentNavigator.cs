// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Softeq.XToolkit.WhiteLabel.Extensions;

namespace Softeq.XToolkit.WhiteLabel.Navigation.FluentNavigators
{
    /// <summary>
    ///     Navigator service that helps to setup and perform opening dialogs for the specified ViewModel.
    /// </summary>
    /// <typeparam name="TViewModel">Type of ViewModel to perform navigation to.</typeparam>
    public class DialogFluentNavigator<TViewModel> : FluentNavigatorBase<TViewModel>
        where TViewModel : class, IDialogViewModel
    {
        private readonly IDialogsService _dialogsService;

        private bool _awaitResult;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DialogFluentNavigator{TViewModel}"/> class.
        /// </summary>
        /// <param name="dialogsService">
        ///     The <see cref="IDialogsService"/> implementation that will be used for opening dialogs.
        /// </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="dialogsService"/> cannot be <see langword="null"/>.
        /// </exception>
        public DialogFluentNavigator(IDialogsService dialogsService)
        {
            _dialogsService = dialogsService
                ?? throw new ArgumentNullException(nameof(dialogsService));
        }

        /// <summary>
        ///     Creates and adds navigation parameter for the specified property using reflection.
        /// </summary>
        /// <typeparam name="TValue">Type of parameter value.</typeparam>
        /// <param name="propertyExpression">Target property.</param>
        /// <param name="value">Value to set.</param>
        /// <returns>Self.</returns>
        /// <exception cref="T:System.ArgumentNullException">
        ///     <paramref name="propertyExpression"/> cannot be <see langword="null"/>.
        /// </exception>
        public DialogFluentNavigator<TViewModel> WithParam<TValue>(
            Expression<Func<TViewModel, TValue>> propertyExpression,
            TValue value)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            ApplyParameter(propertyExpression, value);
            return this;
        }

        /// <summary>
        ///     Use when you need to get the result faster than dialog will be dismissed.
        ///     Avoid using a queue of dialogs in this case.
        /// </summary>
        /// <returns>Self.</returns>
        public DialogFluentNavigator<TViewModel> WithAwaitResult()
        {
            _awaitResult = true;
            return this;
        }

        /// <summary>
        ///     Opens a dialog for the previously specified ViewModel and awaites until this dialog is dismissed.
        /// </summary>
        /// <returns>Task.</returns>
        public Task Navigate()
        {
            return _dialogsService
                .ShowForViewModelAsync<TViewModel>(Parameters)
                .WaitUntilDismissed();
        }

        /// <summary>
        ///     Opens a dialog for the previously specified ViewModel and awaites until this dialog is dismissed.
        ///     For awaiting result use method <see cref="WithAwaitResult"/>.
        /// </summary>
        /// <typeparam name="TResult">Type of dialog result.</typeparam>
        /// <returns>Task with the dialog result.</returns>
        public async Task<TResult> Navigate<TResult>()
        {
            var showDialogTask = _dialogsService.ShowForViewModelAsync<TViewModel, TResult>(Parameters);

            if (_awaitResult)
            {
                var result = await showDialogTask;
                return result.Value;
            }

            return await showDialogTask.WaitUntilDismissed();
        }
    }
}
